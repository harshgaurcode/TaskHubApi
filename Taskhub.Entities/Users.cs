namespace Taskhub.Entities
{
    public class Users
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public string Email { get; set; }   

        public string PhoneNumber {get; set; }
     
        public string UserProfile { get; set; }

        public string Signature { get; set; }   

        public string Address { get; set; }

        public string AlternateContact { get; set; }

        public string UserFamily { get; set; }

      

        public DateTime CreatedTimeStamp { get; set; }

        public DateTime UpdatedTimeStamp { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }



    }
}