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
            var item = _context.PhonebookEntries.Find(id);

            var details = _context.ContactDetails.Where(d => d.PhonebookEntryId == item.PhonebookEntryId).ToList();
            details.ForEach(d => { d.PhonebookEntry = null; });
            item.ContactDetails = details;

            return item;
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
        public PhonebookEntry Put(PhonebookEntry entry)
        {
            var result = _context.Update(entry);

            var detailsForEntry =
                _context.ContactDetails.Where(c => c.PhonebookEntryId == result.Entity.PhonebookEntryId);
            var ids = result.Entity.ContactDetails.Select(d => d.ContactDetailId).ToList();
            if (!ids.Any())
            {
                _context.ContactDetails.RemoveRange(detailsForEntry);
                Done();
            }
            else
            {
                foreach (var d in detailsForEntry)
                {
                    if (ids.Contains(d.ContactDetailId)) continue;
                    _context.ContactDetails.Remove(d);
                    Done();
                }
            }

            Done();

            foreach (var entityContactDetail in result.Entity.ContactDetails)
            {
                entityContactDetail.PhonebookEntry = null;
            }

            return result.Entity;
        }

        /// <summary>
        /// Add phonebook entry
        /// </summary>
        /// <param name="entry"></param>
        public PhonebookEntry Post(PhonebookEntry entry)
        {
            var result = _context.PhonebookEntries.Add(entry);
            Done();
            foreach (var entityContactDetail in result.Entity.ContactDetails)
            {
                entityContactDetail.PhonebookEntry = null;
            }

            return result.Entity;
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