using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EqualizeTimestamps {
	public class Equalizer {
		private string Source { get; set; }
		private string Target { get; set; }
		private ILogger Logger { get; set; }

		public Equalizer(string source, string target, ILogger logger) {
			this.Source = source;
			this.Target = target;
			this.Logger = logger ?? new SilentLogger();
		}

		private void Equalize(FileSystemInfo sourceEntry, FileSystemInfo targetEntry) {
			Logger.Log(targetEntry.FullName);
			var restoreReadOnly = false;

			if ((targetEntry.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) {
				// Remove flag, mess with the file, then restore
				targetEntry.Attributes = targetEntry.Attributes & (~FileAttributes.ReadOnly);
				restoreReadOnly = true;
			}

			try {
				targetEntry.CreationTime = sourceEntry.CreationTime;
				targetEntry.LastAccessTime = sourceEntry.LastAccessTime;
				targetEntry.LastWriteTime = sourceEntry.LastWriteTime;
			}
			finally {
				if (restoreReadOnly) {
					targetEntry.Attributes = targetEntry.Attributes | FileAttributes.ReadOnly;
				}
			}
		}

		public void Run() {
			var sourceDir = new DirectoryInfo(this.Source);
			if (!sourceDir.Exists) {
				throw new Exception($"Source directory doesn't exist: ${this.Source}");
			}
			var targetDir = new DirectoryInfo(this.Target);
			if (!targetDir.Exists) {
				throw new Exception($"Target directory doesn't exist: ${this.Target}");
			}

			var sourceEntries = sourceDir.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly).ToList();

			var targetEntries = new Dictionary<string, FileSystemInfo>();
			foreach (var targetEntry in targetDir.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly)) {
				targetEntries.Add(targetEntry.Name, targetEntry);
			}

			foreach (var sourceEntry in sourceEntries) {
				if (!targetEntries.ContainsKey(sourceEntry.Name)) {
					continue;
				}

				var targetEntry = targetEntries[sourceEntry.Name];

				var sourceIsDir = (sourceEntry.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
				var targetIsDir = (targetEntry.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
				if (sourceIsDir && targetIsDir) {
					// Recurse into directories
					// TODO: If too deep for stack, add to a queue, then consume later
					Logger.Depth++;
					new Equalizer(sourceEntry.FullName, targetEntry.FullName, Logger).Run();
					Logger.Depth--;
				} else if (!sourceIsDir && !targetIsDir) {
					// If neither is dir, equalize dates
					Equalize(sourceEntry, targetEntry);
				}
			}

			// Finally, fix up directories themselves
			Equalize(sourceDir, targetDir);
		}
	}
}

