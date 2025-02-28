using Aot.debug;
using Cysharp.Threading.Tasks;
using UnityEngine;


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

		async private UniTask start_game() {
			Debugger.Log("start_game");
			await UniTask.Delay(5000);
			Debug.Log("after 5s");
		}
	}
}