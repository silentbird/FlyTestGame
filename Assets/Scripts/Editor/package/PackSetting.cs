using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "打包设置/Pack Settings")]
public class PackSetting : ScriptableObject {
    [SerializeField]
    public List<BundlePolicy> policies = new List<BundlePolicy>();

    [Serializable]
    public class BundlePolicy {
        [Tooltip("资源路径模式（支持*通配符）")]
        public string pathPattern;

        [Header("打包规则")]
        public PackingMode packingMode;

        public CompressionType compression = CompressionType.Lz4;

        [Header("热更类型")]
        public HotUpdateType hotUpdateType;

        [Tooltip("是否递归子目录")]
        public bool recursive;
    }

    public enum PackingMode {
        SingleFile, // 每个文件单独打包
        MergeFolder, // 合并整个目录
        GroupByType // 按文件类型分组
    }

    public enum HotUpdateType {
        BuiltIn, // 包内资源
        HotUpdate, // 热更资源
        DynamicDownload // 动态下载资源
    }
}