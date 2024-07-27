using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Entities
{
    public class TaskhubDbContext:DbContext
    {
        public TaskhubDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Users> Users { get; set; }

        public DbSet<UserloginInforamtion> UserloginInforamtions { get; set; }

        public DbSet<UserOfficalInformation> UserOfficalInformations { get; set; }

        public DbSet<LoginTimeStamp> LoginTimeStamps { get; set; }


    }
}
