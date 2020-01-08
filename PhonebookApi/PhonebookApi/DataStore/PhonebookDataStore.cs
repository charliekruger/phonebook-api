using System;
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

        public PhonebookDataStore()
        {
            
        }
        
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
            try
            {
                var items = _context?.PhonebookEntries.ToList();

                items?.ForEach(i =>
                {
                    var details = _context?.ContactDetails.Where(d => d.PhonebookEntryId == i.PhonebookEntryId).ToList();
                    details?.ForEach(d => { d.PhonebookEntry = null; });

                    i.ContactDetails = details;
                });

                return items;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get specific phonebook entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PhonebookEntry</returns>
        public PhonebookEntry Get(int id)
        {
            try
            {
                var item = _context?.PhonebookEntries.Find(id);

                var details = _context?.ContactDetails.Where(d => d.PhonebookEntryId == item.PhonebookEntryId).ToList();
                details?.ForEach(d => { d.PhonebookEntry = null; });
                if (item != null)
                {
                    item.ContactDetails = details;
                }

                return item;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Delete specific phonebook entry
        /// </summary>
        /// <param name="entry"></param>
        public void Delete(PhonebookEntry entry)
        {
            try
            {
                _context?.Remove(entry);
                Done();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Update phonebook entry
        /// </summary>
        /// <param name="entry"></param>
        public PhonebookEntry Put(PhonebookEntry entry)
        {
            try
            {
                var result = _context?.Update(entry);

                var detailsForEntry =
                    _context?.ContactDetails.Where(c => c.PhonebookEntryId == result.Entity.PhonebookEntryId);
                var ids = result?.Entity.ContactDetails.Select(d => d.ContactDetailId).ToList();
                if (ids != null && !ids.Any())
                {
                    _context?.ContactDetails.RemoveRange(detailsForEntry);
                    Done();
                }
                else
                {
                    if (detailsForEntry != null)
                    {
                        foreach (var d in detailsForEntry)
                        {
                            if (ids != null && ids.Contains(d.ContactDetailId)) continue;
                            _context?.ContactDetails.Remove(d);
                            Done();
                        }
                    }
                }

                Done();

                if (result == null) return null;
                
                foreach (var entityContactDetail in result.Entity.ContactDetails)
                {
                    entityContactDetail.PhonebookEntry = null;
                }

                return result.Entity;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Add phonebook entry
        /// </summary>
        /// <param name="entry"></param>
        public PhonebookEntry Post(PhonebookEntry entry)
        {
            try
            {
                var result = _context?.PhonebookEntries.Add(entry);
                Done();
                
                if (result?.Entity.ContactDetails == null) return result?.Entity;
                
                foreach (var entityContactDetail in result?.Entity.ContactDetails)
                {
                    entityContactDetail.PhonebookEntry = null;
                }
                
                return result?.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get all phonebook entries
        /// </summary>
        /// <returns></returns>
        private List<PhonebookEntry> GetAllEntries()
        {
            try
            {
                return _context?.PhonebookEntries.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Save changes to db
        /// </summary>
        private void Done()
        {
            try
            {
                _context?.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Clean db
        /// Testing method only
        /// </summary>
        public void CleanDb()
        {
            try
            {
                _context?.PhonebookEntries.RemoveRange(_context?.PhonebookEntries);
                Done();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}