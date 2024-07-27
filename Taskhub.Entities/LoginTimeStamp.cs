namespace Taskhub.Entities
{
    public class LoginTimeStamp
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public DateTime LoggedInTimeStamp { get; set; }

        public DateTime LoggedOutTimeStamp { get; set;}

        public DateTime LastLoginTimeStamp { get; set; }
        
        public Guid UserId { get; set; }

        public int ActiveStatus { get; set; }

    }
}