namespace AwsDotnetCsharp
{
    public class AppointmentRequest
    {
        public string PracticeId { get; set; }
        public string PatientName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string AppointmentId { get; set; }
        public string Ticket { get; set; }
        public User User { get; set; }
    }

    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class AppointmentResponse
    {
        public string Ticket { get; set; }
        public string AppointmentId { get; set; }
        public string ResponseBody { get; set; }

    }

}