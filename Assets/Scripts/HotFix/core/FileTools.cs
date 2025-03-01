using System.IO;
using Aot.debug;

namespace HotFix.core {
	public static class FileTools {
		public static void CopyFile(string src, string dst) {
			if (!File.Exists(src)) {
				Debugger.LogError("File not found: " + src);
				return;
			}

			string directoryName = Path.GetDirectoryName(dst);
			if (string.IsNullOrEmpty(directoryName) || Directory.Exists(directoryName))
				return;
			Directory.CreateDirectory(directoryName);
			File.Copy(src, dst, true);
		}
	}
}