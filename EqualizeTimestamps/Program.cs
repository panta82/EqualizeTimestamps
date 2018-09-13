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

		[Option('v', "verbose", Default = false, HelpText = "Verbose output")]
		public bool Verbose { get; set; }
	}

	class Program {
		static void Main(string[] args) {
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(options => {
					var logger = new Logger(options.Verbose ? LoggerLevel.Verbose : LoggerLevel.Info);
					new Equalizer(options.Source, options.Target, logger).Run();
				});
		}
	}
}
