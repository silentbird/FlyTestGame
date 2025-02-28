using Aot.debug;

namespace Aot.load {
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