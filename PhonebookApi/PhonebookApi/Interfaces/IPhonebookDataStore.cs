using System.Collections.Generic;
using PhonebookApi.DTO;

namespace PhonebookApi
{
    /// <summary>
    /// Interface to interact with database
    /// </summary>
    public interface IPhonebookDataStore
    {
        List<PhonebookEntry> GetAll();
        PhonebookEntry Get(int id);
        void Delete(PhonebookEntry entry);
        void Put(PhonebookEntry entry);
        void Post(PhonebookEntry entry);
        void CleanDb();
    }
}