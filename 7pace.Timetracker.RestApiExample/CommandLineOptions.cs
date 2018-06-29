using System;
using System.Collections.Generic;
using CommandLine;

namespace _7pace.Timetracker.RestApiExample
{
    public class CommandLineOptions
    {
        [Value( 0, Required = true, HelpText = "Service URL for Timetracker API endpoint (without ?api-version)" )]
        public string RestApiUrl { get; set; }

        [Option( 'w', Default = false, HelpText = "On-premise usage (NTLM auth)" )]
        public bool IsWindowsAuth { get; set; }

        [Option( 't', HelpText = "Token for Timetracker API (VSTS usage)" )]
        public string Token { get; set; }
    }
}