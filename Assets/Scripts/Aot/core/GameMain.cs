using System.IO;
using Aot.debug;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


namespace Aot.core {
	/// <summary>
	/// 游戏主入口 - AOT部分
	/// </summary>
	public class GameMain : MonoBehaviour {
		private void Awake() {
			DontDestroyOnLoad(gameObject);

			Debug.Log("GameMain Awake");
		}

		private void Start() {
			Debug.Log("GameMain Start");

			//start_game
			UniTask.Create(start_game);
		}

		private async UniTask load_hotfix() {
			var dll = (await UnityWebRequest.Get(Path.Combine(Application.persistentDataPath, "hotfix.dll")).SendWebRequest()).downloadHandler.data;
		}

		private async UniTask start_game() {
			Debugger.Log("start_game");
			await load_hotfix();
			Debug.Log("after 5s");
		}
	}
}