using Amazon.Lambda.APIGatewayEvents;
using AwsDotnetCsharp;
using Newtonsoft.Json;

namespace AppointmentControllerTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var control = new AppointmentController();
            AppointmentController.GenerateUniqueDigits("W");
            var appointment = new AppointmentRequest
            {
                PracticeId = "1",
                Type = "W",
                PatientName = "Bailey",
                User = new User()
                {
                    FirstName = "paritosh",
                    EmailAddress = "paritoshmmmec@gmail.com",
                    LastName = "baghel",
                    UserId = "paritoshmmmec@gmail.com"

                },
                
                Description = "bailey has a respiratory problem and has been having a difficult time"
            };
            var request = JsonConvert.SerializeObject(appointment);
            var createdApi = control.Add(new APIGatewayProxyRequest
            {
                Body = request
            }, null).Result;

            var response = JsonConvert.DeserializeObject<AppointmentResponse>(createdApi.Body);

            //var data = control.Update(new APIGatewayProxyRequest()
            //{
            //    Body =JsonConvert.SerializeObject(new AppointmentRequest()
            //    {
            //        Ticket = response.Ticket,
            //        AppointmentId = response.AppointmentId,
            //        PracticeId = "DE-1",
            //    })
            //}, null).Result;

        }
    }
}