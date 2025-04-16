using System.Collections.Generic;
using Aot.debug;
using HotFix.module.ui.core;

namespace HotFix.module.load {
	public class UIModule : ModuleBase {
		public static readonly Dictionary<string, UIPanel> _uiPanels;

		public void Init() {
			Debugger.Log("LoadModule Init");
		}

		public void Start() {
			Debugger.Log("LoadModule Start");
		}

		public void Update() {
		}

		public static void Open(int viewId) {
		}
	}
}