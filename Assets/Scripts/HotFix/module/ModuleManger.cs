using System.Collections.Generic;
using Aot.core;
using HotFix.module.load;
using UnityEngine;

namespace HotFix.module {
	public class ModuleManger : Singleton<ModuleManger> {
		private readonly HashSet<ModuleBase> m_modules = new();

		public void Init() {
			Debug.Log("ModuleManger Init");
			Add<AssetModule>();
			Add<TouchModule>();
			Add<SceneModule>();
		}

		public void Start() {
			foreach (var module in m_modules) module.Start();
		}

		public void Add<T>(bool enable = true) where T : ModuleBase, new() {
			if (!enable) return;

			var module = new T();
			m_modules.Add(module);
		}

		public void StartGame() {
			SceneModule.ChangeSceneAsync(SceneConst.SCENE_LOGIN);
		}
	}
}