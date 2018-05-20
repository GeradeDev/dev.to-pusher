using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Jose;
using Newtonsoft.Json;
using JsonSerializer = Amazon.Lambda.Serialization.Json.JsonSerializer;

namespace AwsDotnetCsharp
{
    public partial class AppointmentController
    {
        public static object locker = new object();

        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<APIGatewayProxyResponse> Add(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var appointmentRequest = JsonConvert.DeserializeObject<AppointmentRequest>(request.Body);
            var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);

            var appointmentId = Guid.NewGuid().ToString();
            var ticketNumber = GenerateUniqueDigits(appointmentRequest.Type);
            var putItemRequest = new PutItemRequest
            {
                TableName = "Appointment",
                Item = new Dictionary<string, AttributeValue>
                {
                    {"PracticeId", new AttributeValue {S = appointmentRequest.PracticeId}},
                    {"UserId", new AttributeValue {S = appointmentRequest.User.EmailAddress}},
                    {"Description", new AttributeValue {S = appointmentRequest.Description}},
                    {"AppointmentId", new AttributeValue {S = appointmentId}},
                    {"FirstName", new AttributeValue() {S = appointmentRequest.User.FirstName} },
                    {"LastName", new AttributeValue() {S = appointmentRequest.User.LastName} },
                    {"EmailAddress", new AttributeValue() {S = appointmentRequest.User.EmailAddress} },
                    {"PatientName", new AttributeValue() {S = appointmentRequest.PatientName} },
                    {"TicketNumber", new AttributeValue {S = ticketNumber}},
                    {"Type",  new AttributeValue {S = appointmentRequest.Type}},
                    {"IsServed", new AttributeValue(){S= "false"}}
                }
            };

            await client.PutItemAsync(putItemRequest);
            var waitTime = await CalculateTime(appointmentRequest.PracticeId);

            var calculatedTime = JsonConvert.SerializeObject(new
            {
                WaitTime = waitTime,
            });

            var userFeed = JsonConvert.SerializeObject(new
            {
                TicketNumber = ticketNumber,
                FirstName = appointmentRequest.User.FirstName.Substring(0,1),
                LastName = appointmentRequest.User.LastName.Substring(0, 1),
            });

            Publish("WaitTimeFeed-" + appointmentRequest.PracticeId, new List<string>() {calculatedTime});
            Publish("Feed-" + appointmentRequest.PracticeId, new List<string>() { userFeed });


            return new APIGatewayProxyResponse
            {
                Headers = new Dictionary<string, string>()
                {
                    { "Access-Control-Allow-Origin","*"},
                    {"access-control-allow-methods","*"}
                },
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(new AppointmentResponse
                {
                    AppointmentId = appointmentId,
                    Ticket = ticketNumber,
                    ResponseBody = request.Body
                })
            };
        }

        public void Publish(string feedId, List<string> messages)
        {
            //TODO remove key
            var baseUrl = "https://us1.pusherplatform.io/services/feeds/v1";
            var url = string.Format("{0}/8550d385-1b9a-4ed5-90ad-a2f1aa29dffa/feeds/{1}/items", baseUrl, feedId);
            var token = GetToken();
            using (var httpClient = new HttpClient())
            {
                var items =new FeedMessage()
                {
                    Items = messages.ToArray()
                };

                var serializedcontent = JsonConvert.SerializeObject(items);
                var content = new StringContent(serializedcontent, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var taskResult= httpClient.PostAsync(url, content).Result;
                if (taskResult.IsSuccessStatusCode)
                {
                    return;
                }
                return;
            }
        }
        public string GetToken()
        {
            var createdTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var expiryTime = (int)DateTime.UtcNow.AddHours(2).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var payload = new PusherToken()
            {
                App = "8550d385-1b9a-4ed5-90ad-a2f1aa29dffa",
                Exp = expiryTime,
                Iat = createdTime,
                
                Iss = "api_keys/***pusher-key-id***",
                Feeds = new Feeds()
                {
                    Permission = new Permission()
                    {
                        Action = "*",
                        Path = "*"
                    }
                }
            };

            var secretKey = Encoding.ASCII.GetBytes("***pusher-secret-key***"); 
            return JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
        }
       
        public async Task<int> CalculateTime(string practiceId)
        {
            var tableName = "Appointment";
            var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
            var appointmentTable = Table.LoadTable(client, tableName);

            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("IsServed", ScanOperator.Equal, "false");
            scanFilter.AddCondition("PracticeId", ScanOperator.Equal, practiceId);

            var searchResult = appointmentTable.Scan(scanFilter);
            List<Document> documentList = new List<Document>();
            do
            {
                var documents = await searchResult.GetNextSetAsync();
                documentList.AddRange(documents);
            } while (!searchResult.IsDone);

            return documentList.Count * 15;
        }

        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<APIGatewayProxyResponse> Update(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var appointmentRequest = JsonConvert.DeserializeObject<AppointmentRequest>(request.Body);
            var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);

            var appointmentId = appointmentRequest.AppointmentId;
            var productCatalog = Table.LoadTable(client, "Appointment");

            var appointment = new Document
            {
                ["AppointmentId"] = appointmentId,
                ["PracticeId"] = appointmentRequest.PracticeId,
                ["IsServed"]= "true"
            };

            var config = new UpdateItemOperationConfig
            {
                // Get updated item in response.
                ReturnValues = ReturnValues.AllNewAttributes
            };
            await  productCatalog.UpdateItemAsync(appointment, config);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "success"
            };
        }

        public static string GenerateUniqueDigits(string type)
        {
            lock (locker)
            {
                return type + (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            }
        }
    }
}