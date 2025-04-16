using Aot.debug;
using HotFix.module.ui.core;

namespace HotFix.module.load {
	public class LoadModule : ModuleBase,IUIBehavior {
		public void Init() {
			Debugger.Log("LoadModule Init");
		}

		public void Start() {
			Debugger.Log("LoadModule Start");
		}

		public void Update() {
		}

		public static void Main(string[] args) {
			Debugger.Log("LoadModule Start");
		}

		private T LoadAsset<T>(string path) where T : class {
			return default;
		}
	}
}