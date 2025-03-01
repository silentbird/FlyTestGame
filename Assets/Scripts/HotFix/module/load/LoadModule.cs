using Aot.debug;

namespace HotFix.module.load {
	public class LoadModule : IModuleBase {
		public void Init() {
			Debugger.Log("LoadModule Init");
		}

		public void Start() {
			Debugger.Log("LoadModule Start");
		}

		public void Update() {
		}
	}
}