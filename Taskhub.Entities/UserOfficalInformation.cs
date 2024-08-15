using System.ComponentModel.DataAnnotations.Schema;

namespace Taskhub.Entities
{
    public class UserOfficalInformation
    {
        public Guid Id { get; set; }

        [ForeignKey("Users")]
        public Guid UserId { get; set; }

        public Users? Users { get; set; }

        public Guid ManagedBy { get; set; }

        public DateTime JoiningDate { get; set; }

        public bool IsBouns { get; set; }

        public int BounsAmmount { get; set; }

        public int Salary { get; set; }

        public int Review { get; set; }

        public string ReviewDiscription { get; set; }=String.Empty;

        public DateTime CreatedTimeStamp { get; set; }

        public DateTime UpdatedTimeStamp { get; set; }


        
    }
}