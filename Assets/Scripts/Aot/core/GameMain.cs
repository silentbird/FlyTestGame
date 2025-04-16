using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aot.debug;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


namespace Aot.core {
	/// <summary>
	/// 游戏主入口 - AOT部分
	/// </summary>
	public class GameMain : MonoBehaviour {
		public List<string> hotfixDlls;
		private readonly Dictionary<string, Assembly> _assemblies = new();

		private void Awake() {
			DontDestroyOnLoad(gameObject);

			Debug.Log("GameMain Awake");
		}

		private async void Start() {
			Debug.Log("GameMain Start");

			//start_game
			//TODO 把启动加载hotfix流程搬到单独模块实现
			await start_game();
		}

		#region 启动加载hotfix

		private void editor_load_hotfix() {
			foreach (var dll in hotfixDlls) {
				Assembly a = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == dll);
				if (a == null) {
					throw new Exception($"{dll} is nil");
				}

				Debugger.Log("editor_load_hotfix: " + dll);
				_assemblies.TryAdd(dll, a);
			}
		}

		private async UniTask app_load_hotfix() {
			foreach (var dll in hotfixDlls) {
				try {
					//TODO 临时读取StreamingAssets下的dll
					var path = Path.Combine(Application.streamingAssetsPath, $"asm/hotfix/{dll}.dll");
					var bytes = (await UnityWebRequest.Get(path).SendWebRequest()).downloadHandler.data;
					var ass = Assembly.Load(bytes);
					Debugger.Log("app_load_hotfix: " + dll);
					_assemblies.TryAdd(dll, ass);
				}
				catch (Exception e) {
					Debugger.LogError(e.Message);
					throw;
				}
			}
		}

		private async UniTask start_hotfix() {
			if (_assemblies.TryGetValue("hotfix", out Assembly ass)) {
				Type entryType = ass.GetType("HotFix.core.HotFixEntry");
				if (entryType == null) {
					throw new Exception("HotFixEntry is nil");
				}

				MethodInfo method = entryType.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);
				if (method == null) {
					throw new Exception("HotFixEntry:Start is nil");
				}

				method.Invoke(null, null);
			}
		}

		#endregion

		private async UniTask start_game() {
			Debugger.Log("start_game");
#if UNITY_EDITOR
			editor_load_hotfix();
#else
			await app_load_hotfix();
#endif
			await start_hotfix();
		}
	}
}