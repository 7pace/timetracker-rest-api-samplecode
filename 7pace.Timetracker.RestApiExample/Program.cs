using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace _7pace.Timetracker.RestApiExample
{
    class Program
    {
        private const string apiRootUrl = "https://sample.com/api";
        private const string apiVersionStr = "api-version";
        private const string apiVersion = "3.0-preview";
        private const string apiRestRoot = "rest";
        private const string apiMeEndpoint = "me";
        private const string apiUserEndpoint = "users";
        private const string apiActivityTypeEndpoint = "activityTypes";
        private const string apiWorkLogsEndpoint = "workLogs";

        private const string authToken = "7h_51xkA3lkpNC3c4aE498KlglS_9KpDJIeynAZ9HT7XqejaxtYQS2_0lZb8eIyDP4_HZdgz5kCyP73AIisIuo9-j18tKWtG1DP1MgHc4Ev1Mgf3nYgnwObRznqeyNYxTdSPcRETF7Kl6XYYLaQz46BQhwkTs78C-bRM2u657ZrEL8zd8GsA31-W9tZCSNZD-d1rT7b01_qemZfEnBoKx3aXLeuJ3fhYmldFzYtkXaWndtXlC2CDb7BAxNq9MAtpCrhrwkO8_tYndGxDJ1gFlzRSbQGAEaqyd8tA_kzh71jtdyyUPJUImFKQ1IgKtuhYE4vLSqAmYcdfXS704bEjDTrVSIdmaqHD2Lb02Dn5iaj8a5oKHedMj7IDWPB1kiOxEL-Vmfr9Mht9Nk4wr3nxPq8LuMqoxS3_VKjZj5NRfdDq7nDNGpNxmp0HK89W7ELG";

        static void Main ( string[] args )
        {
            MainAsync( args ).GetAwaiter().GetResult();
        }

        private static async Task MainAsync ( string[] args )
        {
            await QueryGetAndPrint( apiMeEndpoint );
            await QueryGetAndPrint( apiUserEndpoint );
            await QueryGetAndPrint( apiActivityTypeEndpoint );
            await QueryGetAndPrint( apiWorkLogsEndpoint, new Dictionary<string, string>()
            {
                { "$fromTimestamp", "2018-05-01" },
                { "$count", "10" }
            } );

            Console.WriteLine( "Finished. Press any key to close" );
            Console.ReadKey();
        }

        private static IFlurlRequest GetBase ()
        {
            return new Url( apiRootUrl )
                   .AppendPathSegment( apiRestRoot )
                   .SetQueryParam( apiVersionStr, apiVersion )
                   .WithOAuthBearerToken( authToken );
        }

        private static async Task QueryGetAndPrint ( string path, Dictionary<string, string> queryStringParameters = null )
        {
            Console.WriteLine( $"Query /{path}" );
            var queryRequest = GetBase().AppendPathSegment( path );
            if ( queryStringParameters != null )
            {
                foreach ( KeyValuePair<string, string> queryStringParameter in queryStringParameters )
                {
                    queryRequest = queryRequest.SetQueryParam( queryStringParameter.Key, queryStringParameter.Value );
                }
            }

            var queryResult = await queryRequest.GetStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject( queryResult );
            Console.WriteLine( deserializedObject );
            Console.WriteLine( "\r\nPress any key to continue\r\n" );
            Console.ReadKey();
        }
    }
}