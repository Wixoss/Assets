using System;
using UnityEditor;
using UnityEngine;

namespace OnGUIWrappers
{
	public class GUIArea : IDisposable
	{
		public GUIArea(Rect area)
		{
			GUILayout.BeginArea(area);
		}

		public GUIArea(Rect area, string content)
		{
			GUILayout.BeginArea(area, content);
		}

		public GUIArea(Rect area, string content, string style)
		{
			GUILayout.BeginArea(area, content, style);
		}

		public void Dispose()
		{
			GUILayout.EndArea();
		}
	}
	/// <summary>
	/// Scroll property exposes the current scroll which must be saved in your own editor script.
	/// ie: using (var view = new GUIScrollView(_myScrollPos)) {
	/// _myScrollPos = view.Scroll; (...)
	/// </summary>
	public class GUIScrollView : IDisposable
	{
		public Vector2 Scroll
		{
			get;
			set;
		}

		public GUIScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
		{
			Scroll = GUILayout.BeginScrollView(scrollPosition, options);
		}

		public GUIScrollView(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
		{
			Scroll = GUILayout.BeginScrollView(scrollPosition, style, options);

		}

		public GUIScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollBar, GUIStyle verticalScrollBar, params GUILayoutOption[] options)
		{
			Scroll = GUILayout.BeginScrollView(scrollPosition, horizontalScrollBar, verticalScrollBar, options);
		}

		public void Dispose()
		{
			GUILayout.EndScrollView();
		}
	}

	public class GUIVertical : IDisposable
	{
		public GUIVertical()
		{
			GUILayout.BeginVertical();
		}

		public GUIVertical(params GUILayoutOption[] layoutOptions)
		{
			GUILayout.BeginVertical(layoutOptions);
		}
		public GUIVertical(GUIStyle style, params GUILayoutOption[] layoutOptions)
		{
			GUILayout.BeginVertical(style, layoutOptions);
		}

		public void Dispose()
		{
			GUILayout.EndVertical();
		}
	}

	public class GUIHorizontal : IDisposable
	{
		public GUIHorizontal()
		{
			GUILayout.BeginHorizontal();
		}

		public GUIHorizontal(GUIStyle style, params GUILayoutOption[] layoutOptions)
		{
			GUILayout.BeginHorizontal(style, layoutOptions);
		}
		public GUIHorizontal(params GUILayoutOption[] layoutOptions)
		{
			GUILayout.BeginHorizontal(layoutOptions);
		}

		public void Dispose()
		{
			GUILayout.EndHorizontal();
		}
	}

	public class GUIEnable : IDisposable
	{
		[SerializeField]
		private bool PreviousState
		{
			get;
			set;
		}

		public GUIEnable(bool newState)
		{
			PreviousState = GUI.enabled;
			GUI.enabled = newState;
		}

		public void Dispose()
		{
			GUI.enabled = PreviousState;
		}
	}

	public class GizmosColor : IDisposable
	{
		[SerializeField]
		private Color PreviousColor
		{
			get;
			set;
		}

		public GizmosColor(Color newColor)
		{
			PreviousColor = GUI.color;
			Gizmos.color = newColor;
		}

		public void Dispose()
		{
			Gizmos.color = PreviousColor;
		}
	}
	public class HandlesColor : IDisposable
	{
		[SerializeField]
		private Color PreviousColor
		{
			get;
			set;
		}

		public HandlesColor(Color newColor)
		{
			PreviousColor = GUI.color;
			Handles.color = newColor;
		}

		public void Dispose()
		{
			Handles.color = PreviousColor;
		}
	}
	public class GUIColor : IDisposable
	{
		[SerializeField]
		private Color PreviousColor
		{
			get;
			set;
		}

		public GUIColor(Color newColor)
		{
			PreviousColor = GUI.color;
			GUI.color = newColor;
		}

		public void Dispose()
		{
			GUI.color = PreviousColor;
		}
	}
	public class GUIBackgroundColor : IDisposable
	{
		[SerializeField]
		private Color PreviousColor
		{
			get;
			set;
		}

		public GUIBackgroundColor(Color newColor)
		{
			PreviousColor = GUI.backgroundColor;
			GUI.backgroundColor = newColor;
		}

		public void Dispose()
		{
			GUI.backgroundColor = PreviousColor;
		}
	}

	public class GUIContentColor : IDisposable
	{
		[SerializeField]
		private Color PreviousColor
		{
			get;
			set;
		}

		public GUIContentColor(Color newColor)
		{
			PreviousColor = GUI.contentColor;
			GUI.contentColor = newColor;
		}

		public void Dispose()
		{
			GUI.contentColor = PreviousColor;
		}
	}
}