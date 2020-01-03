using System.Collections.Generic;
using System.Linq;
using PhonebookApi.DTO;

namespace PhonebookApi.DataStore
{
    /// <summary>
    /// Interact with database
    /// </summary>
    public class PhonebookDataStore : IPhonebookDataStore
    {
        private readonly PhonebookContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public PhonebookDataStore(PhonebookContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all phonebook entries
        /// </summary>
        /// <returns>List of PhonebookEntry</returns>
        public List<PhonebookEntry> GetAll()
        {
            var items = _context.PhonebookEntries.ToList();

            items.ForEach(i =>
            {
                var details = _context.ContactDetails.Where(d => d.PhonebookEntryId == i.PhonebookEntryId).ToList();
                details.ForEach(d => { d.PhonebookEntry = null; });

                i.ContactDetails = details;
            });

            return items;
        }

        /// <summary>
        /// Get specific phonebook entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PhonebookEntry</returns>
        public PhonebookEntry Get(int id)
        {
            return _context.PhonebookEntries.Find(id);
        }

        /// <summary>
        /// Delete specific phonebook entry
        /// </summary>
        /// <param name="entry"></param>
        public void Delete(PhonebookEntry entry)
        {
            _context.Remove(entry);
            Done();
        }

        /// <summary>
        /// Update phonebook entry
        /// </summary>
        /// <param name="entry"></param>
        public void Put(PhonebookEntry entry)
        {
            _context.Update(entry);
            Done();
        }

        /// <summary>
        /// Add phonebook entry
        /// </summary>
        /// <param name="entry"></param>
        public void Post(PhonebookEntry entry)
        {
            _context.PhonebookEntries.Add(entry);
            Done();
        }

        /// <summary>
        /// Get all phonebook entries
        /// </summary>
        /// <returns></returns>
        private List<PhonebookEntry> GetAllEntries()
        {
            return _context.PhonebookEntries.ToList();
        }

        /// <summary>
        /// Save changes to db
        /// </summary>
        private void Done()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Clean db
        /// Testing method only
        /// </summary>
        public void CleanDb()
        {
            _context.PhonebookEntries.RemoveRange(_context.PhonebookEntries);
            Done();
        }
    }
}