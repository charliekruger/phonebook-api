using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PhonebookApi.Controllers;
using PhonebookApi.DataStore;
using PhonebookApi.DTO;
using PhonebookApi.Services;

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

            var dataStore = new PhonebookDataStore(context);
            var logger = new Logger<ContactsController>(new LoggerFactory());
            var phonebookService = new PhonebookService(dataStore, logger);

            _controller = new ContactsController(phonebookService, logger);
            _controller.CleanDb();
        }

        [Test]
        public void Post()
        {
            var items = InitPhonebookEntries();

            var actionResult = _controller.Get();

            var listItems = ((OkObjectResult) actionResult.Result).Value as List<PhonebookEntry>;

            var allAdded = true;

            listItems?.ForEach(i =>
            {
                if (allAdded)
                {
                    allAdded = items.Contains(i);
                }
            });

            Assert.IsTrue(allAdded);
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult.Result);
        }

        [Test]
        public void PostReturnsOk()
        {
            var mockService = new Mock<PhonebookService>();

            var controller = new ContactsController(mockService.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Post(new PhonebookEntry()
            {
                Name = "Test",
                Surname = "Test",
                ContactDetails = new List<ContactDetail>
                {
                    new ContactDetail
                    {
                        Content = "0821231234",
                        Description = "Description"
                    }
                }
            });

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
        }

        [Test]
        public void PostReturnsBadRequest()
        {
            var mockService = new Mock<PhonebookService>();

            var controller = new ContactsController(mockService.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Post(null);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), actionResult);
        }

        [Test]
        public void DeleteReturnsOk()
        {
            var getResult = InitPhonebookEntries();

            var itemToDelete = getResult[0];

            var actionResult = _controller.Delete(itemToDelete.PhonebookEntryId);

            var getActionResult = _controller.Get();
            var listItems = ((OkObjectResult) getActionResult.Result).Value as List<PhonebookEntry>;

            Assert.IsFalse(listItems != null && listItems.Contains(itemToDelete));
            Assert.IsInstanceOf(typeof(OkResult), actionResult);
        }

        [Test]
        public void PutReturnsOk()
        {
            var newName = "TEST NAME";
            var surname = "TEST SURNAME";

            var items = InitPhonebookEntries();

            var itemToUpdate = items[0];

            itemToUpdate.Name = newName;
            itemToUpdate.Surname = surname;

            var actionResult = _controller.Put(itemToUpdate);

            var updatedItem = _controller.Get(itemToUpdate.PhonebookEntryId);

            Assert.IsTrue(updatedItem.Value == itemToUpdate);
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult.Result);
        }

        [Test]
        public void PutReturnsBadRequest()
        {
            var mockService = new Mock<PhonebookService>();

            var controller = new ContactsController(mockService.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Put(null);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), actionResult.Result);
        }

        [Test]
        public void GetReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<PhonebookService>();

            var controller = new ContactsController(mockService.Object,
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
            var mockService = new Mock<PhonebookService>();

            var controller = new ContactsController(mockService.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        [Test]
        public void GetReturnsBadRequest()
        {
            var mockService = new Mock<PhonebookService>();

            var controller = new ContactsController(mockService.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Get(0);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), actionResult.Result);
        }

        [Test]
        public void DeleteReturnsBadRequest()
        {
            var mockService = new Mock<PhonebookService>();
            var controller = new ContactsController(mockService.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Delete(0);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), actionResult);
        }

        private void AddItemsForTests()
        {
            var items = TestHelper.GetMockEntries();

            items.ForEach(item => _controller.Post(item));
        }

        private List<PhonebookEntry> InitPhonebookEntries()
        {
            var actionResult = _controller.Get();
            var items = (OkObjectResult) actionResult.Result;

            if (items.Value != null)
            {
                var entries = (List<PhonebookEntry>) items.Value;

                if (entries.Any()) return entries;
            }

            AddItemsForTests();
            var addedActionResult = _controller.Get();
            var addedItems = (OkObjectResult) addedActionResult.Result;

            return addedItems.Value as List<PhonebookEntry>;
        }
    }
}