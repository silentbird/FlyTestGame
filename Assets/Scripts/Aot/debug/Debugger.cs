using UnityEngine;

namespace Aot.debug {
	public static class Debugger {
		public static void Log(string str) {
			Debug.Log(str);
		}

		public static void LogError(string str) {
			Debug.LogError(str);
		}
	}
}