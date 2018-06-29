using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommandLine;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace _7pace.Timetracker.RestApiExample
{
    class Program
    {
        private const string apiVersionParameter = "api-version";
        private const string apiVersionValue = "3.0-preview";

        private const string apiRestRoot = "rest";
        private const string apiMeEndpoint = "me";
        private const string apiUserEndpoint = "users";
        private const string apiActivityTypeEndpoint = "activityTypes";
        private const string apiWorkLogsEndpoint = "workLogs";

        static void Main ( string[] args )
        {
            try
            {
                MainAsync( args ).GetAwaiter().GetResult();
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
            }
            finally
            {
                Console.WriteLine( "Finished. Press any key to close" );
                Console.ReadKey();
            }
        }

        private static CommandLineOptions Configuration;

        private static async Task MainAsync ( string[] args )
        {
            ConsoleDualOut.Init( "appLog.json" );
            Console.WriteLine( $"Execution started at {DateTime.Now}" );
            bool parametersParsed = false;

            CommandLine.Parser.Default.ParseArguments<CommandLineOptions>( args ).WithParsed( x =>
                                                                                              {
                                                                                                  parametersParsed = true;
                                                                                                  Configuration = x;
                                                                                              } )
                       .WithNotParsed( x => { Console.WriteLine( "Check https://github.com/7pace/timetracker-rest-api-samplecode to get samples of usage" ); } );

            if ( !parametersParsed )
            {
                Console.ReadLine();
                return;
            }

            //GET "https://[timetrackerUrl]/api/rest/me?api-version=3.0-preview
            await GetAndPrint<object>( apiMeEndpoint );

            //GET "https://[timetrackerUrl]/api/rest/users?api-version=3.0-preview
            await GetAndPrint<object>( apiUserEndpoint );

            //GET "https://[timetrackerUrl]/api/rest/activityTypes?api-version=3.0-preview
            await GetAndPrint<object>( apiActivityTypeEndpoint );

            //GET "https://[timetrackerUrl]/api/rest/workLogs?$fromTimestamp=2018-05-01&$count=10&api-version=3.0-preview
            await GetAndPrint<object>( apiWorkLogsEndpoint, new Dictionary<string, string>()
            {
                { "$fromTimestamp", "2018-05-01" },
                { "$count", "10" }
            } );

            //POST "https://[timetrackerUrl]/api/rest/workLogs?api-version=3.0-preview; JSON body
            var newWorkLog = await VerbAndPrint<EntityWithId>( HttpMethod.Post, apiWorkLogsEndpoint, new Dictionary<string, string>()
            {
                { "length", "3600" },
                { "comment", "test created" }
            } );

            //PATCH "https://[timetrackerUrl]/api/rest/workLogs/{id}?api-version=3.0-preview; JSON body
            var updatedWorklog = await VerbAndPrint<EntityWithId>( new HttpMethod( "PATCH" ), new[] { apiWorkLogsEndpoint, newWorkLog.Data.Id.ToString() }, new Dictionary<string, string>()
            {
                { "length", "7200" },
                { "comment", "test updated" }
            } );

            //DELETE "https://[timetrackerUrl]/api/rest/workLogs/{id}?api-version=3.0-preview
            await VerbAndPrint<EntityWithId>( HttpMethod.Delete, new[] { apiWorkLogsEndpoint, updatedWorklog.Data.Id.ToString() } );
        }

        private static IFlurlRequest GetBaseUrl ()
        {
            //build url similar to "https://[timetrackerUrl]/api/rest/[action]?api-version=3.0-preview
            var url = new Url( Configuration.RestApiUrl )
                      .AppendPathSegment( apiRestRoot )
                      .SetQueryParam( apiVersionParameter, apiVersionValue );
            if ( Configuration.IsWindowsAuth )
            {
                var credentials = CredentialCache.DefaultNetworkCredentials;
                return url.WithBasicAuth( credentials.UserName, credentials.Password );
            }

            return url.WithOAuthBearerToken( Configuration.Token );
        }

        private static IFlurlRequest GetRequest ( string[] paths, Dictionary<string, string> queryStringParameters = null )
        {
            var queryRequest = GetBaseUrl();
            queryRequest = paths.Aggregate( queryRequest, ( current, s ) => current.AppendPathSegment( s ) );
            if ( queryStringParameters == null )
            {
                return queryRequest;
            }

            foreach ( KeyValuePair<string, string> queryStringParameter in queryStringParameters )
            {
                queryRequest = queryRequest.SetQueryParam( queryStringParameter.Key, queryStringParameter.Value );
            }

            return queryRequest;
        }

        private static Task<SuccessResponse<T>> GetAndPrint<T> ( string path, Dictionary<string, string> queryStringParameters = null )
        {
            return GetAndPrint<T>( new[] { path }, queryStringParameters );
        }

        private static async Task<SuccessResponse<T>> GetAndPrint<T> ( string[] paths, Dictionary<string, string> queryStringParameters = null )
        {
            var queryRequest = GetRequest( paths, queryStringParameters );
            Console.WriteLine( $"GET {queryRequest.Url}" );

            var queryResult = await queryRequest.GetStringAsync();
            SuccessResponse<T> result = JsonConvert.DeserializeObject<SuccessResponse<T>>( queryResult );

            Console.WriteLine( JsonConvert.DeserializeObject( queryResult ) );
            Console.WriteLine( "\r\nPress any key to continue\r\n" );
            Console.ReadKey();
            return result;
        }

        private static Task<SuccessResponse<T>> VerbAndPrint<T> ( HttpMethod verb, string path, Dictionary<string, string> bodyParameters = null )
        {
            return VerbAndPrint<T>( verb, new[] { path }, bodyParameters );
        }

        private static async Task<SuccessResponse<T>> VerbAndPrint<T> ( HttpMethod verb, string[] paths, Dictionary<string, string> bodyParameters = null )
        {
            var queryRequest = GetRequest( paths );
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
                Console.WriteLine( $"{verb} {queryRequest.Url}" );
                var responseString = await queryResult.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<SuccessResponse<T>>( responseString );
                Console.WriteLine( JsonConvert.DeserializeObject( responseString ) );
            }
            else
            {
                Console.WriteLine( $"{verb} is not supported" );
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

    public class EntityWithId
    {
        public Guid Id { get; set; }
    }
}