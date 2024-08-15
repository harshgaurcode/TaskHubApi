using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Taskhub.Entities
{
    public class Users
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public string Email { get; set; }   =string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
     
        public string? UserProfile { get; set; }

        public string? Signature { get; set; }   

        public string? Address { get; set; }

        public string AlternateContact { get; set; }=string.Empty;

        public string AlteranateContactOf { get; set; }=string.Empty;

        public bool IsAuthenticated { get; set; } = false;


        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];

        public DateTime? LastLogin { get; set; }
        public bool IsAvailable { get; set; } = false;

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }

        //Navigation 
        [NotMapped]
        public UserRole? Role { get; set; }

        [NotMapped]
        public ICollection<ProjectMembers>? ProjectMembers { get; set; }

        [NotMapped]
        public UserOfficalInformation? UserOfficialInformation { get; set; }


        public DateTime CreatedTimeStamp { get; set; }

        public DateTime UpdatedTimeStamp { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }



    }
}