using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace AwsDotnetCsharp
{
    public class WaitListCalculator
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public APIGatewayProxyResponse Calculate(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var practiceId = QueryStringFormatter.Get(request.QueryStringParameters, "practiceName");
            context.Logger.LogLine(string.Format("Got request for ${0}", practiceId));
            return new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = string.Format("{0} {1}", "Yippe! practiceId", practiceId)
            };
        }
    }
}