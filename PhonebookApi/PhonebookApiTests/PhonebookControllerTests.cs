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
            var mockRepository = new Mock<PhonebookDataStore>();

            var controller = new ContactsController(mockRepository.Object,
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
            var mockRepository = new Mock<PhonebookDataStore>();

            var controller = new ContactsController(mockRepository.Object,
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

            Assert.IsFalse(listItems.Contains(itemToDelete));
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
            var mockRepository = new Mock<PhonebookDataStore>();

            var controller = new ContactsController(mockRepository.Object,
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

        [Test]
        public void GetReturnsBadRequest()
        {
            var mockRepository = new Mock<PhonebookDataStore>();

            var controller = new ContactsController(mockRepository.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Get(0);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), actionResult.Result);
        }

        [Test]
        public void DeleteReturnsBadRequest()
        {
            var mockRepository = new Mock<PhonebookDataStore>();
            var controller = new ContactsController(mockRepository.Object,
                new Logger<ContactsController>(new LoggerFactory()));

            // Act
            var actionResult = controller.Delete(0);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), actionResult);
        }

        private List<PhonebookEntry> AddItemsForTests()
        {
            var items = TestHelper.GetMockEntries();

            items.ForEach(item => _controller.Post(item));

            return items;
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