using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace PhonebookApi.DTO
{
    /// <summary>
    /// Database context for phonebook entries and detail
    /// </summary>
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