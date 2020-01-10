using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PhonebookApi.Controllers;
using PhonebookApi.DataStore;
using PhonebookApi.DTO;
using PhonebookApi.Interfaces;
using PhonebookApi.Services;

namespace PhonebookApiTests
{
    public class PhonebookServiceTests
    {
        private IPhonebookService _phonebookService;
        private PhonebookDataStore _dataStore;

        [SetUp]
        public void Setup()
        {
            var context = new PhonebookContext();
            context.Database.EnsureCreated();

            var logger = new Logger<ContactsController>(new LoggerFactory());

            _dataStore = new PhonebookDataStore(context);
            _dataStore.CleanDb();

            _phonebookService = new PhonebookService(_dataStore, logger);
        }

        private void AddItemsForTests()
        {
            var items = TestHelper.GetMockEntries();

            items.ForEach(item => _phonebookService.Post(item));
        }

        private List<PhonebookEntry> InitPhonebookEntries()
        {
            var items = _phonebookService.GetAll();

            if (items.Any()) return items;

            AddItemsForTests();
            items = _phonebookService.GetAll();

            return items;
        }

        [Test]
        public void AddItems()
        {
            var items = InitPhonebookEntries();

            var addedItems = _phonebookService.GetAll();

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

            _phonebookService.Delete(itemToDelete);

            items = _phonebookService.GetAll();
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

            _phonebookService.Put(itemToUpdate);

            var updatedItem = _phonebookService.Get(itemToUpdate.PhonebookEntryId);

            Assert.IsTrue(updatedItem == itemToUpdate);
        }
    }
}