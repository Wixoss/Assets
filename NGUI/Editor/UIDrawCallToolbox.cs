using System.Collections.Generic;
using OnGUIWrappers;
using UnityEngine;
using UnityEditor;


//TODO needs a rework to function properly with the new NGUI.
public class UIDrawCallToolbox : EditorWindow
{
    private RecompileCheckClass _recompile;

    private class RecompileCheckClass
    {
    }

    [MenuItem("NGUI/Open/Nicki's Draw Call Toolbox")]
    static public void OpenPanelOverview()
    {
        EditorWindow.GetWindow<UIDrawCallToolbox>(false, "Draw Calls", true);
    }

    //TODO make fold out all draw calls button when they're not.
    Vector2 mScroll = Vector2.zero;

    readonly Dictionary<string, int> _depths = new Dictionary<string, int>();
    private bool _depthsDirty = false;


    readonly Color32[] _allColors =
    {
	    new Color32(124,252,0,255),        //lawn green
	    new Color32(30,144,255,255),         //dodger blue
	    new Color32(255,215,32,255),         //gold
	    new Color32(255,0,255,255),          //magenta
	    new Color32(244,164,96,255),            //sandy brown
	    new Color32(230,230,250,255),        //lavender
	    new Color32(0,128,0,255),            //green
	    new Color32(186,85,211,255),         //medium orchid
        
        
	    //add more colors for more differentiation
	    //see http://www.rapidtables.com/web/color/RGB_Color.htm#rgb-format
    };
    private readonly Dictionary<UIPanel, Color32> _activeColors = new Dictionary<UIPanel, Color32>();


    void OnGUI()
    {
        if (_recompile == null) //has recompiled.
        {
            _recompile = new RecompileCheckClass();
            _depthsDirty = true;
            _activeColors.Clear();
            GUI.FocusControl(null);
            //other things that need to reset?
        }
        if (_depthsDirty)
        {
            _depths.Clear();
            _depthsDirty = false;
        }
        var dcs = UIDrawCall.activeList;
        dcs.Sort((a, b) => a.finalRenderQueue.CompareTo(b.finalRenderQueue));

        using (new GUIVertical())
        {
            using (var view = new GUIScrollView(mScroll))
            {
                mScroll = view.Scroll;
                for (int i = 0; i < UIDrawCall.activeList.size; ++i)
                {
                    OnGUI_DrawCall(dcs, i);
                }
            }
        }
    }

    private void OnGUI_DrawCall(BetterList<UIDrawCall> dcs, int drawCallIndex)
    {
        UIDrawCall dc = dcs[drawCallIndex];
        string key = "Draw Call " + (drawCallIndex + 1);


        Color32 backgroundColor;

        if (_activeColors.ContainsKey(dc.panel))
        {
            backgroundColor = _activeColors[dc.panel];
        }
        else
        {
            backgroundColor = _allColors[_activeColors.Count % _allColors.Length];
            _activeColors.Add(dc.panel, backgroundColor);
        }

        if (!dc.isActive)
        {
            backgroundColor = Color.red;
        }

        GUI.backgroundColor = backgroundColor;


        string dcName = "<b>" + key + " of " + dcs.size + " (" + dc.panel.name + ")</b>";
        if (!dc.isActive)
        {
            dcName = dcName + " <b>(HIDDEN)</b>";
        }
        bool shouldBeUnfolded = DrawHeader(dcName, key);
        if (!shouldBeUnfolded)
        {
            return;
        }

        NGUIEditorTools.BeginContents();
        GUI.backgroundColor = Color.white;
        EditorGUILayout.ObjectField("Material", dc.baseMaterial, typeof(Material), false);
        string panelKey = key + "panel";

        int currentPanelDepth = dc.panel.depth;
        int panelDepth = currentPanelDepth;
        using (new GUIHorizontal())
        {
            if (_depths.ContainsKey(panelKey))
            {
                panelDepth = _depths[panelKey];
            }
            GUILayout.Label("Panel depth", GUILayout.Width(80f));
            _depths[panelKey] = EditorGUILayout.IntField(panelDepth, GUILayout.Width(50f));

            var alignBefore = GUI.skin.button.alignment;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            if (GUILayout.Button("Select panel: " + dc.panel.name, GUILayout.ExpandWidth(false), GUILayout.MinWidth(250f)))
            {
                Selection.activeGameObject = dc.panel.gameObject;
            }
            GUI.skin.button.alignment = alignBefore;
            GUILayout.Space(18f);
        }

        using (new GUIHorizontal())
        {
            GUILayout.Label("Hide draw call", GUILayout.Width(90f));

            bool draw = !EditorGUILayout.Toggle(!dc.isActive);

            if (dc.isActive != draw)
            {
                dc.isActive = draw;
                EditorUtility.SetDirty(dc.panel);
            }
        }


        if (panelDepth != currentPanelDepth)
        {
            OnGUI_DepthModifiedButtons(dc, panelDepth, panelKey, currentPanelDepth);
        }
        var depths = new List<List<UIWidget>>();

        int initial = NGUITools.GetHierarchy(dc.panel.cachedGameObject).Length + 1;
        int masterListIndex = -1;
        int currentDepth = int.MinValue;

        for (int a = 0; a < UIPanel.list.Count; ++a)
        {
            UIPanel p = UIPanel.list[a];

            for (int b = 0; b < p.widgets.Count; ++b)
            {
                UIWidget w = p.widgets[b];

                int depth = w.depth;
                if (currentDepth == int.MinValue || currentDepth != depth)
                {
                    currentDepth = depth;
                    masterListIndex++;
                    depths.Add(new List<UIWidget>());
                }

                if (w.drawCall == dc)
                {
                    depths[masterListIndex].Add(w);
                }
            }
        }


        for (int d = 0; d < depths.Count; d++)
        {
            int widgetCount = depths[d].Count;
            if (widgetCount < 1)
            {
                continue;
            }
            currentDepth = depths[d][0].depth;

            string foldoutTitle = widgetCount + " Widget" + (widgetCount == 1 ? "" : "s") + " - Depth: " + currentDepth.ToString();

            string depthKey = key + "depth" + currentDepth.ToString();

            bool wasFoldedOut = EditorPrefs.GetBool(depthKey, false);

            int massDepth = currentDepth;
            if (_depths.ContainsKey(depthKey))
            {
                massDepth = _depths[depthKey];
            }

            bool foldedOut = false;
            using (new GUIHorizontal())
            {
                GUILayout.Label("Depth", GUILayout.Width(40f));
                _depths[depthKey] = EditorGUILayout.IntField(massDepth, GUILayout.Width(50f));


                foldedOut = DrawDepthCollapser("<b>" + foldoutTitle + "</b> - Click to " + (wasFoldedOut ? "collapse" : "expand"), depthKey, wasFoldedOut);
            }


            if (massDepth != currentDepth)
            {
                OnGUI_WidgetDepthModified(depths, d, massDepth, depthKey, dc, currentDepth);
            }
            if (foldedOut)
            {
                for (int iW = 0; iW < depths[d].Count; iW++)
                {
                    using (new GUIHorizontal())
                    {
                        GUILayout.Space(10f);
                        var alignBefore = GUI.skin.button.alignment;
                        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                        if (GUILayout.Button(NGUITools.GetHierarchy(depths[d][iW].cachedGameObject).Remove(0, initial), GUILayout.ExpandWidth(false)))
                        {
                            Selection.activeGameObject = depths[d][iW].gameObject;
                        }
                        GUI.skin.button.alignment = alignBefore;
                    }
                }
            }
        }
        NGUIEditorTools.EndContents();
    }

    private void OnGUI_WidgetDepthModified(List<List<UIWidget>> depths, int d, int massDepth, string depthKey, UIDrawCall dc, int currentDepth)
    {
        using (new GUIHorizontal())
        {
            using (new GUIBackgroundColor(new Color(0.4f, 1f, 0.4f)))
            {
                if (GUILayout.Button("Apply Depth to widgets", GUILayout.Width(150f)))
                {
                    for (int iW = 0; iW < depths[d].Count; iW++)
                    {
                        depths[d][iW].depth = massDepth;
                    }
                    EditorPrefs.DeleteKey(depthKey);
                    _depthsDirty = true;
                    GUI.FocusControl(null);
                    EditorUtility.SetDirty(dc.panel);
                }
            }
            using (new GUIBackgroundColor(new Color(1f, 0.8f, 0.8f)))
            {
                if (GUILayout.Button("Reset Depth", GUILayout.Width(150f)))
                {
                    _depths[depthKey] = currentDepth;
                    GUI.FocusControl(null);
                    EditorUtility.SetDirty(dc.panel);
                }
            }
        }
    }

    private void OnGUI_DepthModifiedButtons(UIDrawCall dc, int panelDepth, string panelKey, int currentPanelDepth)
    {
        using (new GUIHorizontal())
        {
            using (new GUIBackgroundColor(new Color(0.4f, 1f, 0.4f)))
            {
                if (GUILayout.Button("Apply Depth to panel", GUILayout.Width(150f)))
                {
                    dc.panel.depth = panelDepth;
                    _depths.Clear();
                    _depthsDirty = true;
                    GUI.FocusControl(null);
                    EditorUtility.SetDirty(dc.panel);
                }
                using (new GUIBackgroundColor(new Color(1f, 0.8f, 0.8f)))
                {
                    if (GUILayout.Button("Reset Depth", GUILayout.Width(150f)))
                    {
                        _depths[panelKey] = currentPanelDepth;
                        GUI.FocusControl(null);
                        EditorUtility.SetDirty(dc.panel);
                    }
                }
            }
        }
    }


    private bool DrawDepthCollapser(string text, string key, bool forceOn)
    {
        bool state = EditorPrefs.GetBool(key, forceOn);

        GUILayout.Space(3f);
        Color oldColor = GUI.backgroundColor;
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.Space(3f);
        GUI.changed = false;
        if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.ExpandWidth(false))) state = !state;
        if (GUI.changed) EditorPrefs.SetBool(key, state);
        GUILayout.Space(2f);
        GUI.backgroundColor = oldColor;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }

    private bool DrawHeader(string text, string key)
    {
        bool state = EditorPrefs.GetBool(key, true);
        GUILayout.Space(3f);
        using (new GUIHorizontal())
        {
            GUILayout.Space(3f);
            GUI.changed = false;
            if (!GUILayout.Toggle(true, text, "dragtab")) state = !state;
            if (GUI.changed) EditorPrefs.SetBool(key, state);
            GUILayout.Space(2f);
        }
        if (!state) GUILayout.Space(3f);
        return state;
    }
}
