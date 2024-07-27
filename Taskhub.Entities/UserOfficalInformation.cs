namespace Taskhub.Entities
{
    public class UserOfficalInformation
    {
        public string Role { get; set; }

        public string RoleName { get; set; }

        public Guid ManagedBy { get; set; }
        public DateTime JoiningDate { get; set; }
    
        public int Salary { get; set; }

        public int Review { get; set; }

        public string ReviewDiscription { get; set; }

        public DateTime CreatedTimeStamp { get; set; }

        public DateTime UpdatedTimeStamp { get; set; }

        public bool IsBouns { get; set; }

        public int BounsAmmount { get; set; }

        
    }
}