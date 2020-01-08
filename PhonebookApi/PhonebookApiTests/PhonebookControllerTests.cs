using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
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
            context.Database.EnsureCreated();

            _controller = new ContactsController(new PhonebookDataStore(context),
                new Logger<ContactsController>(new LoggerFactory()));
            _controller.CleanDb();
        }

        private List<PhonebookEntry> AddItemsForTests()
        {
            var items = TestHelper.GetMockEntries();

            items.ForEach(item => _controller.Post(item));

            return items;
        }

        private List<PhonebookEntry> InitPhonebookEntries()
        {
            ActionResult<List<PhonebookEntry>> items = _controller.Get();

            if (items.Value != null)
            {
                if (items.Value.Any()) return items.Value;
            }

            AddItemsForTests();
            items = _controller.Get();

            return items.Value;
        }

        [Test]
        public void AddItems()
        {
            var items = InitPhonebookEntries();

            var addedItems = _controller.Get();

            bool allAdded = true;

            addedItems.Value.ForEach(i =>
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

            items = _controller.Get().Value;
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

            Assert.IsTrue(updatedItem.Value == itemToUpdate);
        }

        [Test]
        public void GetReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<PhonebookDataStore>();
            var mockLogger = new Mock<Logger<ContactsController>>();

            var controller = new ContactsController(mockRepository.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Get(10);

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult.Result);
        }
        
        [Test]
        public void DeleteReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<PhonebookDataStore>();

            var controller = new ContactsController(mockRepository.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }
    }
}