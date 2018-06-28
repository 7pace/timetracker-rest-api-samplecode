using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace _7pace.Timetracker.RestApiExample
{
    class Program
    {
        private const string apiVersionParameter = "api-version";

        private const string apiRestRoot = "rest";
        private const string apiMeEndpoint = "me";
        private const string apiUserEndpoint = "users";
        private const string apiActivityTypeEndpoint = "activityTypes";
        private const string apiWorkLogsEndpoint = "workLogs";

        private const string apiVersionConfigName = "apiVersion";
        private const string apiRootUrlConfigName = "apiRootUrl";
        private const string authTokenConfigName = "authToken";

        static void Main ( string[] args )
        {
            MainAsync( args ).GetAwaiter().GetResult();
        }

        private static IConfigurationRoot Configuration;

        private static async Task MainAsync ( string[] args )
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath( Directory.GetCurrentDirectory() )
                          .AddJsonFile( "appsettings.json" );

            Configuration = builder.Build();

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
            return new Url( Configuration[apiRootUrlConfigName] )
                   .AppendPathSegment( apiRestRoot )
                   .SetQueryParam( apiVersionParameter, Configuration[apiVersionConfigName] )
                   .WithOAuthBearerToken( Configuration[authTokenConfigName] );
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