#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class PackSettingWindow : EditorWindow {
    private PackSetting _setting;
    private SerializedObject serializedSettings;
    private SerializedProperty policiesProp;
    private Vector2 scrollPosition;
    private readonly string settingPath = "Assets/Scripts/Editor/package/pack_setting.asset";

    [MenuItem("打包/打包设置")]
    public static void ShowWindow() {
        GetWindow<PackSettingWindow>("打包设置");
    }

    private void OnEnable() {
        // 加载或创建设置文件
        _setting = AssetDatabase.LoadAssetAtPath<PackSetting>(settingPath);
        if (_setting == null) {
            Debug.LogError($"读取{settingPath}打包设置失败，请确保存在打包设置文件");
            return;
        }

        serializedSettings = new SerializedObject(_setting);
        policiesProp = serializedSettings.FindProperty("policies");
    }

    private void OnGUI() {
        if (serializedSettings == null) {
            EditorGUILayout.LabelField("请先创建打包设置文件", GUILayout.Width(200));
            if (GUILayout.Button("创建打包设置文件")) {
                _setting = CreateInstance<PackSetting>();
                AssetDatabase.CreateAsset(_setting, settingPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                OnEnable();
            }

            return;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("路径配置", EditorStyles.boldLabel);
        //绘制路径

        EditorGUILayout.EndHorizontal();


        serializedSettings.Update();

        EditorGUILayout.Space();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        DrawPolicyList();
        EditorGUILayout.Space();

        if (GUILayout.Button("添加新规则", GUILayout.Height(30))) {
            AddNewPolicy();
        }

        if (GUILayout.Button("验证所有规则", GUILayout.Height(30))) {
            ValidatePolicies();
        }

        EditorGUILayout.EndScrollView();

        serializedSettings.ApplyModifiedProperties();
    }

    private void DrawPolicyList() {
        for (int i = 0; i < policiesProp.arraySize; i++) {
            var policy = policiesProp.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(policy.FindPropertyRelative("pathPattern"));
            if (GUILayout.Button("×", GUILayout.Width(20))) {
                policiesProp.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(policy.FindPropertyRelative("packingMode"));
            EditorGUILayout.PropertyField(policy.FindPropertyRelative("compression"));
            EditorGUILayout.PropertyField(policy.FindPropertyRelative("hotUpdateType"));
            EditorGUILayout.PropertyField(policy.FindPropertyRelative("recursive"));

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
    }

    private void AddNewPolicy() {
        policiesProp.arraySize++;
        var newPolicy = policiesProp.GetArrayElementAtIndex(policiesProp.arraySize - 1);
        newPolicy.FindPropertyRelative("pathPattern").stringValue = "Art/Example/*";
        newPolicy.FindPropertyRelative("packingMode").enumValueIndex = 0;
        newPolicy.FindPropertyRelative("compression").enumValueIndex = 1;
        newPolicy.FindPropertyRelative("hotUpdateType").enumValueIndex = 0;
        newPolicy.FindPropertyRelative("recursive").boolValue = true;
    }

    private void ValidatePolicies() {
        var error = new StringBuilder();

        // 规则优先级检查
        var patterns = new HashSet<string>();
        foreach (var policy in _setting.policies) {
            if (patterns.Contains(policy.pathPattern)) {
                error.AppendLine($"重复路径规则: {policy.pathPattern}");
            }

            patterns.Add(policy.pathPattern);
        }

        // 路径有效性检查
        foreach (var policy in _setting.policies) {
            var testPath = policy.pathPattern.Replace("*", "test");
            if (!AssetDatabase.IsValidFolder(Path.GetDirectoryName(testPath))) {
                error.AppendLine($"无效路径: {policy.pathPattern}");
            }
        }

        if (error.Length > 0) {
            EditorUtility.DisplayDialog("配置错误", error.ToString(), "确定");
        }
        else {
            EditorUtility.DisplayDialog("验证通过", "所有配置规则有效", "确定");
        }
    }
}
#endif