using System;
using System.Globalization;

namespace EqualizeTimestamps {
	public enum LoggerLevel {
		Error = 0,
		Info = 1,
		Verbose = 2,
	}

	public class Logger {
		public LoggerLevel Level { get; set; }
		public int Depth { get; set; }

		public Logger(LoggerLevel level) {
			this.Level = level;
			this.Depth = 0;
		}

		private void DoLog(string message) {
			var depthPrefix = new String(' ', Depth * 2);
			Console.WriteLine("[{0}] {1}{2}",
				DateTimeOffset.Now.ToString("s", CultureInfo.InvariantCulture),
				depthPrefix,
				message);
		}

		public void Verbose(string message) {
			if (Level >= LoggerLevel.Verbose) {
				this.DoLog(message);
			}
		}

		public void Info(string message) {
			if (Level >= LoggerLevel.Info) {
				this.DoLog(message);
			}
		}

		public void Error(Exception ex) {
			Console.WriteLine(ex.Message);
			Console.WriteLine(ex.StackTrace);
		}
	}
}
