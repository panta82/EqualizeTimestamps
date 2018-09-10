using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace EqualizeTimestamps {
	class Options {
		[Value(0, HelpText = "Source directory", MetaName = "source", Required = true)]
		public string Source { get; set; }

		[Value(1, HelpText = "Target directory", MetaName = "target", Required = true)]
		public string Target { get; set; }

		[Option('s', "silent", Default = false, HelpText = "Silence output")]
		public bool Silent { get; set; }
	}

	class Program {
		static void Main(string[] args) {
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(options => {
					ILogger logger;
					if (options.Silent) {
						logger = new SilentLogger();
					} else {
						logger = new ConsoleLogger();
					}
					new Equalizer(options.Source, options.Target, logger).Run();
				});
		}
	}
}
