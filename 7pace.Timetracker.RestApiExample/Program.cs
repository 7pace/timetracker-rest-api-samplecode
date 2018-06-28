using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

            var newWorkLog = await VerbAndPrint<MinimalWorkLog>( HttpMethod.Post, apiWorkLogsEndpoint, new Dictionary<string, string>()
            {
                { "length", "3600" },
                { "comment", "test created" }
            } );

            var updatedWorklog = await VerbAndPrint<MinimalWorkLog>( new HttpMethod( "PATCH" ), new[] { apiWorkLogsEndpoint, newWorkLog.Data.Id.ToString() }, new Dictionary<string, string>()
            {
                { "length", "7200" },
                { "comment", "test updated" }
            } );

            await VerbAndPrint<MinimalWorkLog>( HttpMethod.Delete, new[] { apiWorkLogsEndpoint, updatedWorklog.Data.Id.ToString() } );

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

        private static Task QueryGetAndPrint ( string path, Dictionary<string, string> queryStringParameters = null )
        {
            return QueryGetAndPrint( new[] { path }, queryStringParameters );
        }

        private static async Task QueryGetAndPrint ( string[] paths, Dictionary<string, string> queryStringParameters = null )
        {
            Console.WriteLine( $"Query GET /{string.Join( "/", paths )}" );
            var queryRequest = GetBase();
            queryRequest = paths.Aggregate( queryRequest, ( current, s ) => current.AppendPathSegment( s ) );
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

        private static Task<SuccessResponse<T>> VerbAndPrint<T> ( HttpMethod verb, string path, Dictionary<string, string> bodyParameters = null )
        {
            return VerbAndPrint<T>( verb, new[] { path }, bodyParameters );
        }

        private static async Task<SuccessResponse<T>> VerbAndPrint<T> ( HttpMethod verb, string[] paths, Dictionary<string, string> bodyParameters = null )
        {
            Console.WriteLine( $"Query {verb} /{string.Join( "/", paths )}" );
            var queryRequest = GetBase();
            queryRequest = paths.Aggregate( queryRequest, ( current, s ) => current.AppendPathSegment( s ) );

            HttpResponseMessage queryResult = null;
            SuccessResponse<T> result = null;

            if ( verb == HttpMethod.Post )
            {
                queryResult = await queryRequest.PostJsonAsync( bodyParameters );
            }
            else if ( verb == new HttpMethod( "PATCH" ) )
            {
                queryResult = await queryRequest.PatchJsonAsync( bodyParameters );
            }
            else if ( verb == HttpMethod.Delete )
            {
                queryResult = await queryRequest.DeleteAsync();
            }
            //todo put is not implemented

            if ( queryResult != null )
            {
                var responseString = await queryResult.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<SuccessResponse<T>>( responseString );
                Console.WriteLine( JsonConvert.DeserializeObject( responseString ) );
            }

            Console.WriteLine( "\r\nPress any key to continue\r\n" );
            Console.ReadKey();
            return result;
        }
    }

    public class SuccessResponse<T>
    {
        public T Data { get; set; }
    }

    public class MinimalWorkLog
    {
        public Guid Id { get; set; }
    }
}