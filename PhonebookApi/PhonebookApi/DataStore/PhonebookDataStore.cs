using System.Collections.Generic;
using System.Linq;
using PhonebookApi.DTO;

namespace PhonebookApi.DataStore
{
    public class PhonebookDataStore : IPhonebookDataStore
    {
        private readonly PhonebookContext _context;

        public PhonebookDataStore(PhonebookContext context)
        {
            _context = context;
        }

        public List<PhonebookEntry> GetAll()
        {
            return _context.PhonebookEntries.ToList();
        }

        public PhonebookEntry Get(int id)
        {
            return _context.PhonebookEntries.Find(id);
        }

        public void Delete(PhonebookEntry entry)
        {
            _context.Remove(entry);
            Done();
        }

        public void Put(PhonebookEntry entry)
        {
            _context.Update(entry);
            Done();
        }

        public void Post(PhonebookEntry entry)
        {
            _context.PhonebookEntries.Add(entry);
            Done();
        }

        private List<PhonebookEntry> GetAllEntries()
        {
            return _context.PhonebookEntries.ToList();
        }

        private void Done()
        {
            _context.SaveChanges();
        }

        public void CleanDb()
        {
            _context.PhonebookEntries.RemoveRange(_context.PhonebookEntries);
            Done();
        }
    }
}