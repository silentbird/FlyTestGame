using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HotFix.core;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;

public class PackTools {
    #region 打包-dll

    [MenuItem("打包/打包dll-aot+hotfix", false, 1012)]
    public static void PackNewPackage() {
        PrebuildCommand.GenerateAll();
        copy_hotfix_dll();
        copy_metadata();
        start_pack();
    }

    [MenuItem("打包/打包dll-hotfix", false, 1013)]
    public static void PackHotfix() {
        CompileDllCommand.CompileDllActiveBuildTarget();
        copy_hotfix_dll();
        //临时测试copy到buildpath
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64) {
            copy_hotfix2buildpath();
        }
    }

    private static void copy_hotfix2buildpath() {
        List<string> all_hotfix_dll = SettingsUtil.HotUpdateAssemblyFilesIncludePreserved;
        foreach (var hotfix_dll in all_hotfix_dll) {
            //当前平台
            var hotUpdateOutputDir =
                SettingsUtil.GetHotUpdateDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget);
            var srcPath = Path.Combine(hotUpdateOutputDir, hotfix_dll);
            var dstPath = Path.Combine("Build/StandaloneWindows64/FlyTestGame_Data/StreamingAssets", "asm/hotfix",
                hotfix_dll);
            FileTools.CopyFile(srcPath, dstPath);
        }
    }

    private static void copy_hotfix_dll() {
        List<string> all_hotfix_dll = SettingsUtil.HotUpdateAssemblyFilesIncludePreserved;
        foreach (var hotfix_dll in all_hotfix_dll) {
            //当前平台
            var hotUpdateOutputDir =
                SettingsUtil.GetHotUpdateDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget);
            var srcPath = Path.Combine(hotUpdateOutputDir, hotfix_dll);
            var dstPath = Path.Combine(Application.streamingAssetsPath, "asm/hotfix", hotfix_dll);
            FileTools.CopyFile(srcPath, dstPath);
        }
    }

    private static void copy_metadata() {
        var all_alt_dll = AOTGenericReferences.PatchedAOTAssemblyList;
        foreach (var dll in all_alt_dll) {
            //当前平台
            var hotUpdateOutputDir =
                SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);
            var srcPath = Path.Combine(hotUpdateOutputDir, dll);
            var dstPath = Path.Combine(Application.streamingAssetsPath, "asm/metadata", dll);
            FileTools.CopyFile(srcPath, dstPath);
        }
    }

    private static void start_pack() {
        var buildPlayerOptions = BuildOptions.CleanBuildCache;
        var target = EditorUserBuildSettings.activeBuildTarget;
        var name = Application.productName;
        var targetPath = $"Build/{target}/{name}";
        var dirctName = Path.GetDirectoryName(targetPath);
        if (File.Exists(dirctName)) {
            Directory.Delete($"Build/{target}", true);
        }

        BuildPipeline.BuildPlayer(new[] { "Assets/Scenes/GameMain.unity" }, targetPath, target, buildPlayerOptions);
    }

    #endregion

    #region 打包-ab

    [MenuItem("打包/根据配置生成AB")]
    public static void BuildWithSettings() {
        var settings = Resources.Load<PackSetting>("Assets/Scripts/PackSettings");
        if (settings == null) {
            Debug.LogError("找不到打包配置文件");
            return;
        }

        // 第一步：清除旧标记
        ClearAllBundleNames();

        // 第二步：应用新规则
        ApplyPackingRules(settings);

        // 第三步：执行打包
        BuildPipeline.BuildAssetBundles(
            "AssetBundles",
            BuildAssetBundleOptions.ChunkBasedCompression,
            EditorUserBuildSettings.activeBuildTarget);
    }

    private static void ClearAllBundleNames() {
        foreach (var assetPath in AssetDatabase.GetAllAssetPaths()) {
            var importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null) {
                importer.assetBundleName = null;
            }
        }
    }

    private static void ApplyPackingRules(PackSetting setting) {
        var allAssets = AssetDatabase.GetAllAssetPaths()
            .Where(p => p.StartsWith("Assets/"))
            .ToList();

        foreach (var policy in setting.policies.OrderByDescending(p => p.pathPattern.Length)) {
            var pattern = policy.pathPattern.Replace("*", ".*");
            var regex = new Regex($"^{pattern}$");

            var matchedAssets = allAssets
                .Where(p => regex.IsMatch(p))
                .ToList();

            ApplyPolicyToAssets(policy, matchedAssets);
        }
    }

    private static void ApplyPolicyToAssets(PackSetting.BundlePolicy policy, List<string> assetPaths) {
        switch (policy.packingMode) {
            case PackSetting.PackingMode.SingleFile:
                foreach (var path in assetPaths) {
                    var importer = AssetImporter.GetAtPath(path);
                    importer.assetBundleName = $"single_{Path.GetFileNameWithoutExtension(path)}";
                    SetHotUpdateTag(importer, policy.hotUpdateType);
                }

                break;

            case PackSetting.PackingMode.MergeFolder:
                var folderName = Path.GetDirectoryName(policy.pathPattern)
                    .Replace("Assets/", "")
                    .Replace("/", "_");

                foreach (var path in assetPaths) {
                    var importer = AssetImporter.GetAtPath(path);
                    importer.assetBundleName = $"merge_{folderName}";
                    SetHotUpdateTag(importer, policy.hotUpdateType);
                }

                break;

            case PackSetting.PackingMode.GroupByType:
                var groups = assetPaths
                    .GroupBy(p => Path.GetExtension(p).ToLower());

                foreach (var group in groups) {
                    foreach (var path in group) {
                        var importer = AssetImporter.GetAtPath(path);
                        importer.assetBundleName = $"type_{group.Key.TrimStart('.')}";
                        SetHotUpdateTag(importer, policy.hotUpdateType);
                    }
                }

                break;
        }
    }

    private static void SetHotUpdateTag(AssetImporter importer, PackSetting.HotUpdateType type) {
        // 添加自定义元数据
        importer.SetAssetBundleNameAndVariant(
            importer.assetBundleName,
            type.ToString().ToLower());
    }

    #endregion
}