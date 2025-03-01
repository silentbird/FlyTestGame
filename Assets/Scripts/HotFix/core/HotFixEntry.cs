using System.Collections.Generic;
using System.IO;
using Aot.debug;
using Cysharp.Threading.Tasks;
using HotFix.module;
using HybridCLR;

namespace HotFix.core {
	public class HotFixEntry {
		private static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string> {
			"System.Core.dll",
			"UnityEngine.CoreModule.dll",
			"UnityEngine.UI.dll",
			"aot.dll",
			"mscorlib.dll",
		};

		public void Start() {
			ModuleManger.Instance.Init();
			UniTask.Create(LoadMetadata);
		}

		private async UniTask LoadMetadata() {
			foreach (var asm in PatchedAOTAssemblyList) {
				//Get metadata real path
				// Load asm
				string file = Path.Combine(UnityEngine.Application.streamingAssetsPath, asm);
				var bytes = await File.ReadAllBytesAsync(file);
				LoadImageErrorCode result = RuntimeApi.LoadMetadataForAOTAssembly(bytes, HomologousImageMode.SuperSet);
				if (result != LoadImageErrorCode.OK) {
					Debugger.LogError("LoadMetadataForAOTAssembly failed: " + result);
				}
			}
		}
	}
}