using System.Collections.Generic;
using PhonebookApi.DTO;

namespace PhonebookApi.Interfaces
{
    public interface IPhonebookService
    {
        List<PhonebookEntry> GetAll();
        PhonebookEntry Get(int id);
        void Delete(PhonebookEntry entry);
        PhonebookEntry Put(PhonebookEntry entry);
        PhonebookEntry Post(PhonebookEntry entry);
        void CleanDb();
    }
}