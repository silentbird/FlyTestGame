using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aot.debug {
	/// <summary>
	/// 将控制台日志显示到屏幕上
	/// </summary>
	public class ConsoleToScreen : MonoBehaviour {
		// 最大显示行数
		[SerializeField]
		private int maxLines = 50;

		// 单行最大长度
		[SerializeField]
		private int maxLineLength = 120;

		// 字体大小
		[SerializeField]
		private int fontSize = 15;

		// 文本颜色
		[SerializeField]
		private Color textColor = Color.white;

		// 背景颜色
		[SerializeField]
		private Color backgroundColor = new Color(0, 0, 0, 0.5f);

		// 是否显示时间戳
		[SerializeField]
		private bool showTimestamp = true;

		// 日志区域
		[SerializeField]
		private Rect logArea = new Rect(10, 10, 1000, 600);

		// 日志字符串
		private string _logStr = "";

		// 日志行列表
		private readonly List<string> _lines = new List<string>();

		// GUI样式
		private GUIStyle _style;

		// 是否初始化
		private bool _initialized = false;

		private void Awake() {
			// 确保不会被销毁
			DontDestroyOnLoad(this);

			// 添加一些初始日志
			Log($"[ConsoleToScreen] 初始化完成，开始捕获日志", "", LogType.Log);
		}

		Texture2D myTex;

		private void InitStyle() {
			_initialized = true;
			myTex = new Texture2D(1, 1);
			myTex.SetPixel(1, 1, new Color(0.1f, 0.1f, 0.1f, 0.9f));
			myTex.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
			myTex.Apply();
			_style = new GUIStyle(GUI.skin.label);
			_style.normal.background = myTex;
			_style.active.background = myTex;
			_style.hover.background = myTex;
			_style.focused.background = myTex;
			_style.normal.textColor = textColor;
			_style.active.textColor = textColor;
			_style.hover.textColor = textColor;
			_style.focused.textColor = textColor;
			_style.fontSize = fontSize;
			_style.wordWrap = true;
			_style.richText = true;
			_style.alignment = TextAnchor.UpperLeft;
			_style.padding = new RectOffset(10, 10, 10, 10);
			_style.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
		}

		private void OnEnable() {
			// 注册日志回调
			Application.logMessageReceived += Log;
		}

		private void OnDisable() {
			// 注销日志回调
			Application.logMessageReceived -= Log;
		}

		private void Log(string logString, string stackTrace, LogType type) {
			// 添加时间戳
			string timestamp = showTimestamp ? $"[{DateTime.Now:HH:mm:ss.fff}] " : "";

			// 添加日志类型前缀
			string prefix = "";
			switch (type) {
				case LogType.Error:
				case LogType.Exception:
					prefix = "<color=#cc3030>[错误]</color> ";
					break;
				case LogType.Warning:
					prefix = "[警告] ";
					break;
				case LogType.Log:
					prefix = "[信息] ";
					break;
				case LogType.Assert:
					prefix = "[断言] ";
					break;
			}

			// 处理每一行日志
			foreach (var line in logString.Split('\n')) {
				string formattedLine = timestamp + prefix + line;

				if (formattedLine.Length <= maxLineLength) {
					_lines.Add(formattedLine);
					continue;
				}

				// 处理超长行
				var lineCount = formattedLine.Length / maxLineLength + 1;
				for (int i = 0; i < lineCount; i++) {
					if ((i + 1) * maxLineLength <= formattedLine.Length) {
						_lines.Add(formattedLine.Substring(i * maxLineLength, maxLineLength));
					}
					else {
						_lines.Add(formattedLine.Substring(i * maxLineLength, formattedLine.Length - i * maxLineLength));
					}
				}
			}

			// 限制行数
			if (_lines.Count > maxLines) {
				_lines.RemoveRange(0, _lines.Count - maxLines);
			}

			// 更新日志字符串
			_logStr = string.Join("\n", _lines);
		}

		private void OnGUI() {
			if (!_initialized) {
				InitStyle();
			}

			_style.fontSize = fontSize;

			// 调整日志区域大小，根据缩放后的尺寸
			Rect scaledLogArea = new Rect(
				logArea.x,
				logArea.y,
				Screen.width,
				Screen.height
			);

			// 绘制背景
			GUI.color = backgroundColor;
			GUI.Box(scaledLogArea, "");

			// 恢复颜色
			GUI.color = textColor;

			// 绘制日志文本，使用我们自定义的样式
			GUI.Label(scaledLogArea, _logStr, _style);
		}
	}
}