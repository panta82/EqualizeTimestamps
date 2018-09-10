using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EqualizeTimestamps {
	public interface ILogger {
		int Depth { get; set; }
		void Log(string message);
	}

	public class ConsoleLogger : ILogger {
		public int Depth { get; set; }
		public void Log(string message) {
			var depthPrefix = new String(' ', Depth * 2);
			Console.WriteLine("[{0}] {1}{2}",
				DateTimeOffset.Now.ToString("s", CultureInfo.InvariantCulture),
				depthPrefix,
				message);
		}
	}

	public class SilentLogger : ILogger {
		public int Depth { get; set; }

		public void Log(string message) {
			// Nothing
		}
	}
}
