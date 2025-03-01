using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;

[InitializeOnLoad]
public class ToolbarReloadExtend {
	private static Type m_toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

	private static ScriptableObject m_currentToolbar;
	private static GUIStyle m_commonBtnStyle;

	private static GUIContent m_restartBtnContent;
	private const string WAITING_FOR_COMPILE_KEY = "ReloadButton_WaitingForCompile";

	// 是否正在等待编译完成
	private static bool waitingForCompile {
		get {
			int is_waiting = PlayerPrefs.GetInt(WAITING_FOR_COMPILE_KEY);
			return is_waiting == 1;
		}
		set => PlayerPrefs.SetInt(WAITING_FOR_COMPILE_KEY, value ? 1 : 0);
	}

	static ToolbarReloadExtend() {
		EditorApplication.update -= OnUpdate;
		EditorApplication.update += OnUpdate;

		m_restartBtnContent = EditorGUIUtility.IconContent("Refresh");
	}

	private static void OnUpdate() {
		if (m_currentToolbar == null) {
			var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);
			m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
			if (m_currentToolbar != null) {
				FieldInfo root = m_currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
				VisualElement rawRoot = root.GetValue(m_currentToolbar) as VisualElement;
				VisualElement toolbarZone = rawRoot.Q("ToolbarZoneRightAlign");
				VisualElement parent = new VisualElement() {
					style = {
						flexGrow = 1,
						flexDirection = FlexDirection.Row,
					}
				};
				IMGUIContainer container = new IMGUIContainer();
				container.onGUIHandler += OnGUI;
				parent.Add(container);
				toolbarZone.Add(parent);
			}
		}

		if (waitingForCompile) {
			if (!EditorApplication.isCompiling) {
				if (!EditorApplication.isPlaying) {
					Debug.Log("编译完成，开始播放");
					EditorApplication.isPlaying = true;
					waitingForCompile = false;
				}
			}
		}
	}

	private const string Menu_OpenOrCloseErrorPanel = "Tools/重启并编译 %_r";

	[MenuItem(Menu_OpenOrCloseErrorPanel, false, 9999)]
	private static void ReloadGame() {
		if (EditorApplication.isPlaying) {
			Debug.Log("停止播放，准备重新加载");
			EditorApplication.isPlaying = false;
		}

		waitingForCompile = true;
	}

	private static void OnGUI() {
		if (PlayerPrefs.GetInt("ToolbarActiveButton", 1) == 0) {
			return;
		}

		GUILayout.BeginHorizontal();
		if (GUILayout.Button(new GUIContent(EditorGUIUtility.FindTexture("Refresh")))) {
			if (EditorApplication.isPlaying) {
				ReloadGame();
			}
		}

		GUILayout.EndHorizontal();
	}
}