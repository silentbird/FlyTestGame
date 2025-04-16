using System;
using Aot.debug;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace HotFix.module {
	public class SceneModule : ModuleBase {
		private static SceneData _prevScene;
		private static SceneData _currentScene;

		public void Init() {
		}

		public void ChangeScene(string sceneName, bool async = false) {
			if (_currentScene != null) {
				_prevScene = _currentScene;
			}
		}

		public static async UniTask ChangeSceneAsync(string sceneName) {
			if (_currentScene != null) {
				_prevScene = _currentScene;
			}

			if (sceneName != null) {
			}

			await SceneManager.LoadSceneAsync(sceneName).ToUniTask(Progress.Create<float>(p => { Debugger.Log(p.ToString()); }));
		}
	}

	public class SceneData {
		public string sceneType;

		SceneData(string sceneType) {
		}
	}
}