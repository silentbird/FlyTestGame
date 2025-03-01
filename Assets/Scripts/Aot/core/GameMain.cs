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
		private Dictionary<string, Assembly> _assemblies = new();

		private void Awake() {
			DontDestroyOnLoad(gameObject);

			Debug.Log("GameMain Awake");
		}

		private void Start() {
			Debug.Log("GameMain Start");

			//start_game
			UniTask.Create(start_game);
		}

		private void editor_hotfix() {
			foreach (var dll in hotfixDlls) {
				Assembly a = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == dll);
				if (a == null) {
					throw new Exception($"{dll} is nil");
				}

				_assemblies.TryAdd(dll, a);
			}
		}

		private async UniTask load_hotfix() {
			foreach (var dll in hotfixDlls) {
				try {
					var bytes = (await UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, $"ams/hotfix/{dll}.dll")).SendWebRequest()).downloadHandler.data;
					var ass = Assembly.Load(bytes);
					_assemblies.TryAdd(dll, ass);
				}
				catch (Exception e) {
					Console.WriteLine(e);
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

		private async UniTask start_game() {
			Debugger.Log("start_game");
#if UNITY_EDITOR
			editor_hotfix();
#else
			await load_hotfix();
#endif

			await start_hotfix();
		}
	}
}