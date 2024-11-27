using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Store_Management.Common;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store_Management.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Owner Full Name")]
        public string OwnerName { get; set; }

        [Required]
        public string BusinessName { get; set; }




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductMst> ProductMsts { get; set; }
        public DbSet<ModuleMst> ModuleMsts { get; set; }
        public DbSet<CustomerMst> CustomerMsts { get; set; }
        public DbSet<BillsItemTemp> BillsItemTemps { get; set; }
        public DbSet<ProcessBill> ProcessBills { get; set; }

    }
}