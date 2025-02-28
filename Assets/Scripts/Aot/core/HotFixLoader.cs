using Aot.debug;

namespace Aot.core {
	public class HotFixLoader : Singleton<HotFixLoader> {
		private string[] _dlls = {
			"HotUpdate"
		};


		public void StartLoad() {
			foreach (var dll in _dlls) {
				var assembly = System.Reflection.Assembly.Load(dll);
				if (assembly == null) {
					Debugger.LogError("Load assembly failed: " + dll);
					return;
				}

				Debugger.Log("Load assembly success: " + dll);
			}
		}
	}
}