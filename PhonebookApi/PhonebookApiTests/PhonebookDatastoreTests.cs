using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;
using PhonebookApi.DataStore;
using PhonebookApi.DTO;

namespace PhonebookApiTests
{
    public class PhonebookDatastoreTests
    {
     private PhonebookDataStore _dataStore;

        [SetUp]
        public void Setup()
        {
            var context = new PhonebookContext();
            context.Database.EnsureCreated();

            _dataStore = new PhonebookDataStore(context);
            _dataStore.CleanDb();
        }

        private void AddItemsForTests()
        {
            var items = TestHelper.GetMockEntries();

            items.ForEach(item => _dataStore.Post(item));
        }

        private List<PhonebookEntry> InitPhonebookEntries()
        {
            var items = _dataStore.GetAll();

            if (items.Any()) return items;

            AddItemsForTests();
            items = _dataStore.GetAll();

            return items;
        }

        [Test]
        public void AddItems()
        {
            var items = InitPhonebookEntries();

            var addedItems = _dataStore.GetAll();

            var allAdded = true;

            addedItems.ForEach(i =>
            {
                if (allAdded)
                {
                    allAdded = items.Contains(i);
                }
            });

            Assert.IsTrue(allAdded);
        }

        [Test]
        public void DeleteItem()
        {
            var items = InitPhonebookEntries();
            var itemToDelete = items[0];
            
            _dataStore.Delete(itemToDelete);

            items = _dataStore.GetAll();
            Assert.IsFalse(items.Contains(itemToDelete));
        }

        [Test]
        public void UpdateItem()
        {
            const string newName = "TEST NAME";
            const string surname = "TEST SURNAME";

            var items = InitPhonebookEntries();

            var itemToUpdate = items[0];

            itemToUpdate.Name = newName;
            itemToUpdate.Surname = surname;

            _dataStore.Put(itemToUpdate);

            var updatedItem = _dataStore.Get(itemToUpdate.PhonebookEntryId);

            Assert.IsTrue(updatedItem == itemToUpdate);
        }
    }
}