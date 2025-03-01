using System.Collections.Generic;
using System.IO;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;
using File = System.IO.File;

public class PackTools {
	[MenuItem("Tools/打包-新整包", false, 1012)]
	static void PackNewPackage() {
		PrebuildCommand.GenerateAll();
		copy_hotfix_dll();
		copy_metadata();
		start_pack();
	}

	[MenuItem("Tools/打包-热更", false, 1013)]
	static void PackHotfix() {
		CompileDllCommand.CompileDllActiveBuildTarget();
		copy_hotfix_dll();
	}

	private static void copy_hotfix_dll() {
		List<string> all_hotfix_dll = SettingsUtil.HotUpdateAssemblyFilesIncludePreserved;
		foreach (var hotfix_dll in all_hotfix_dll) {
			//当前平台
			var hotUpdateOutputDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget);
			var srcPath = Path.Combine(hotUpdateOutputDir, hotfix_dll);
			var dstPath = Path.Combine(Application.streamingAssetsPath, "asm/hotfix", hotfix_dll);
			File.Copy(srcPath, dstPath, true);
		}
	}

	private static void copy_metadata() {
		var all_alt_dll = AOTGenericReferences.PatchedAOTAssemblyList;
		foreach (var dll in all_alt_dll) {
			//当前平台
			var hotUpdateOutputDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);
			var srcPath = Path.Combine(hotUpdateOutputDir, dll);
			var dstPath = Path.Combine(Application.streamingAssetsPath, "asm/metadata", dll);
			File.Copy(srcPath, dstPath, true);
		}
	}

	private static void start_pack() {
		var buildPlayerOptions = BuildOptions.CleanBuildCache;
		var target = EditorUserBuildSettings.activeBuildTarget;
		var name = Application.productName;
		var targetPath = $"Build/{target}/{name}";
		Directory.Delete($"Build/{target}", true);

		BuildPipeline.BuildPlayer(new[] { "Assets/Scenes/GameMain.unity" }, targetPath, target, buildPlayerOptions);
	}
}