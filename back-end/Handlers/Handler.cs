using Amazon.Lambda.Core;
using System.Net;

using Amazon.Lambda.APIGatewayEvents;

namespace AwsDotnetCsharp
{
    public class Handler
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public APIGatewayProxyResponse Hello(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("This is useful for debugging!");
            return new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "Hello function executed successfully!",
            };
        }
    }
}