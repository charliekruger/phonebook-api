using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace PhonebookApi.DTO
{
    public class PhonebookContext : DbContext
    {
        public DbSet<PhonebookEntry> PhonebookEntries { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=phonebook.db");
        }
    }
}