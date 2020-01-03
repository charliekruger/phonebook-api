using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PhonebookApi.Controllers;
using PhonebookApi.DataStore;
using PhonebookApi.DTO;

namespace PhonebookApiTests
{
    public class PhonebookControllerTests
    {
        private ContactsController _controller;

        [SetUp]
        public void Setup()
        {
            var context = new PhonebookContext();
            context.Database.Migrate();
            context.Database.EnsureCreated();

            _controller = new ContactsController(new PhonebookDataStore(context),
                new Logger<ContactsController>(new LoggerFactory()));
            _controller.CleanDb();
        }

        private List<PhonebookEntry> AddItemsForTests()
        {
            var items = GetMockEntries();

            items.ForEach(item => _controller.Post(item));

            return items;
        }

        private List<PhonebookEntry> InitPhonebookEntries()
        {
            var items = _controller.Get();

            if (items.Any()) return items;

            AddItemsForTests();
            items = _controller.Get();

            return items;
        }

        [Test]
        public void AddItems()
        {
            var items = InitPhonebookEntries();

            var addedItems = _controller.Get();

            bool allAdded = true;

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

            _controller.Delete(itemToDelete.PhonebookEntryId);

            items = _controller.Get();
            Assert.IsFalse(items.Contains(itemToDelete));
        }

        [Test]
        public void UpdateItem()
        {
            var newName = "TEST NAME";
            var surname = "TEST SURNAME";

            var items = InitPhonebookEntries();

            var itemToUpdate = items[0];

            itemToUpdate.Name = newName;
            itemToUpdate.Surname = surname;

            _controller.Put(itemToUpdate);

            var updatedItem = _controller.Get(itemToUpdate.PhonebookEntryId);

            Assert.IsTrue(updatedItem == itemToUpdate);
        }

        private List<PhonebookEntry> GetMockEntries()
        {
            return new List<PhonebookEntry>
            {
                new PhonebookEntry
                {
                    Name = "John",
                    Surname = "Smith",
                    ContactDetails = new List<ContactDetail>
                    {
                        new ContactDetail
                        {
                            Type = ContactType.Number,
                            Content = "0821231234",
                            Description = "Mobile"
                        },
                        new ContactDetail
                        {
                            Type = ContactType.EmailAddress,
                            Content = "johnsmith@mail.com",
                            Description = "Personal"
                        }
                    }
                },
                new PhonebookEntry
                {
                    Name = "Jane",
                    Surname = "Johnson",
                    ContactDetails = new List<ContactDetail>
                    {
                        new ContactDetail
                        {
                            Type = ContactType.Number,
                            Content = "0121231234",
                            Description = "Home"
                        },
                        new ContactDetail
                        {
                            Type = ContactType.EmailAddress,
                            Content = "janej@companymail.com",
                            Description = "Work"
                        }
                    }
                },
                new PhonebookEntry
                {
                    Name = "J Jonah",
                    Surname = "Jameson",
                    ContactDetails = new List<ContactDetail>
                    {
                        new ContactDetail
                        {
                            Type = ContactType.Number,
                            Content = "+27721231234",
                            Description = "Work Mobile"
                        },
                        new ContactDetail
                        {
                            Type = ContactType.EmailAddress,
                            Content = "jjj@dailybugle.com",
                            Description = "Work"
                        }
                    }
                },
            };
        }
    }
}