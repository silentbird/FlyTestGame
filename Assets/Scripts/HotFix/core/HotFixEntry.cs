using System.Collections.Generic;
using System.IO;
using Aot.debug;
using Cysharp.Threading.Tasks;
using HotFix.module;
using HybridCLR;

namespace HotFix.core {
	public static class HotFixEntry {
		private static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string> {
			"System.Core.dll",
			"UnityEngine.CoreModule.dll",
			"UnityEngine.UI.dll",
			"aot.dll",
			"mscorlib.dll",
		};

		public static void Start() {
			ModuleManger.Instance.Init();
			Debugger.Log("这是一个TEst 111111111111111111");
			UniTask.Create(LoadMetadata);
		}

		private static async UniTask LoadMetadata() {
			foreach (var asm in PatchedAOTAssemblyList) {
				string file = Path.Combine(UnityEngine.Application.streamingAssetsPath, "asm/metadata", asm);
				var bytes = await File.ReadAllBytesAsync(file);
				LoadImageErrorCode result = RuntimeApi.LoadMetadataForAOTAssembly(bytes, HomologousImageMode.SuperSet);
				if (result != LoadImageErrorCode.OK) {
					Debugger.LogError("LoadMetadataForAOTAssembly failed: " + result);
				}
			}
		}
	}
}