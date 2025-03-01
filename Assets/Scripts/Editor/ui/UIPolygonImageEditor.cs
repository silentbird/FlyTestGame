using Aot.ui;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIPolygonImage), true)]
[CanEditMultipleObjects]
public class UIPolygonImageEditor : ImageEditor {
	SerializedProperty sidesProperty;
	SerializedProperty rotationProperty;
	SerializedProperty hollowProperty;
	SerializedProperty hollowRatioProperty;
	SerializedProperty progressProperty;
	SerializedProperty barTypeProperty;
	SerializedProperty spriteProperty;

	protected override void OnEnable() {
		base.OnEnable();

		sidesProperty = serializedObject.FindProperty("sides");
		rotationProperty = serializedObject.FindProperty("rotation");
		hollowRatioProperty = serializedObject.FindProperty("hollowRatio");
		progressProperty = serializedObject.FindProperty("_progress");
		barTypeProperty = serializedObject.FindProperty("barType");
		spriteProperty = serializedObject.FindProperty("m_Sprite");
	}


	public override void OnInspectorGUI() {
		serializedObject.Update();
		EditorGUILayout.PropertyField(spriteProperty, new GUIContent("sprite"));
		// 绘制形状设置组
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("形状设置", EditorStyles.boldLabel);
		EditorGUI.indentLevel++;

		EditorGUILayout.PropertyField(sidesProperty, new GUIContent("边数"));
		EditorGUILayout.PropertyField(rotationProperty, new GUIContent("旋转角度"));

		EditorGUI.indentLevel--;

		// 绘制空心设置组
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("空心设置", EditorStyles.boldLabel);
		EditorGUI.indentLevel++;

		EditorGUILayout.PropertyField(hollowRatioProperty, new GUIContent("空心比例"));

		EditorGUI.indentLevel--;

		// 绘制进度设置组
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("进度设置", EditorStyles.boldLabel);
		EditorGUI.indentLevel++;

		EditorGUILayout.PropertyField(barTypeProperty, new GUIContent("进度条类型"));
		EditorGUILayout.PropertyField(progressProperty, new GUIContent("进度条进度"));


		EditorGUI.indentLevel--;

		// 绘制Image基类的属性
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("图像设置", EditorStyles.boldLabel);
		// base.OnInspectorGUI();

		serializedObject.ApplyModifiedProperties();

		// 如果有修改，标记为需要重绘
		if (GUI.changed) {
			foreach (var target in targets) {
				if (target is UIPolygonImage image) {
					image.SetVerticesDirty();
				}
			}
		}
	}
}