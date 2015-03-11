using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Assets.Scripts
{
    [CustomEditor(typeof(ShowDeck))]
    public class MainDeckInspector : Editor
    {
        private ShowDeck _showdeck;
        private Vector2 _pos;

        private void OnEnable()
        {
            _showdeck = target as ShowDeck;
        }

        public override void OnInspectorGUI()
        {
            _pos = EditorGUILayout.BeginScrollView(_pos);
            EditorGUILayout.BeginVertical();

            for (int i = 0; i<_showdeck.MainDeck.Count; i++)
            {
                EditorGUILayout.LabelField("CardId", _showdeck.MainDeck [i].CardId);
                EditorGUILayout.LabelField("CardName", _showdeck.MainDeck [i].CardName);
                EditorGUILayout.LabelField("MyCardType", _showdeck.MainDeck [i].MyCardType.ToString());
                EditorGUILayout.LabelField("MyCardColor", _showdeck.MainDeck [i].MyCardColor.ToString());
                EditorGUILayout.ObjectField("CardTexture", _showdeck.MainDeck [i].CardTexture, typeof(Texture2D), false);
                EditorGUILayout.LabelField("CardDetail", _showdeck.MainDeck [i].CardDetail);
                EditorGUILayout.Toggle("HasBrust", _showdeck.MainDeck [i].HasBrust);
                for (int l =0; l<_showdeck.MainDeck[i].Cost.Count; l++)
                {
                    EditorGUILayout.LabelField("Cost", _showdeck.MainDeck [i].Cost [l].MyEnerType.ToString());
                    EditorGUILayout.IntField("Cost", _showdeck.MainDeck [i].Cost [l].Num);
                }

                for (int j =0; j<_showdeck.MainDeck[i].GrowCost.Count; j++)
                {
                    EditorGUILayout.LabelField("GrowCost", _showdeck.MainDeck [i].GrowCost [j].MyEnerType.ToString());
                    EditorGUILayout.IntField("GrowCost", _showdeck.MainDeck [i].GrowCost [j].Num);
                }

                EditorGUILayout.Toggle("BCanGuard", _showdeck.MainDeck [i].BCanGuard);
                EditorGUILayout.IntField("Level", _showdeck.MainDeck [i].Level);
                EditorGUILayout.IntField("Limit", _showdeck.MainDeck [i].Limit);
                if (_showdeck.MainDeck [i].Type != null)
                {
                    EditorGUILayout.LabelField("Type", _showdeck.MainDeck [i].Type.ToString());
                }
                if (_showdeck.MainDeck [i].TypeOnly != null)
                {
                    EditorGUILayout.LabelField("TypeOnly", _showdeck.MainDeck [i].TypeOnly.ToString());
                }
                EditorGUILayout.IntField("Atk", _showdeck.MainDeck [i].Atk);
                EditorGUILayout.LabelField("--------------------" + (i+1) + "-----------------------");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}
