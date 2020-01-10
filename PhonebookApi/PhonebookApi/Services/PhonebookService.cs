using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PhonebookApi.Controllers;
using PhonebookApi.DTO;
using PhonebookApi.Interfaces;

namespace PhonebookApi.Services
{
    public class PhonebookService : IPhonebookService
    {
        private readonly IPhonebookDataStore _dataStore;
        private readonly ILogger _logger;

        public PhonebookService()
        {
        }

        public PhonebookService(IPhonebookDataStore dataStore, ILogger<ContactsController> logger)
        {
            _logger = logger;
            _dataStore = dataStore;
        }
        
        public List<PhonebookEntry> GetAll()
        {
            try
            {
                return _dataStore?.GetAll();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on GetAll");
                Console.WriteLine(e);
                throw;
            }
        }

        public PhonebookEntry Get(int id)
        {
            try
            {
                return _dataStore?.Get(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on Get");
                Console.WriteLine(e);
                throw;
            }
        }

        public void Delete(PhonebookEntry entry)
        {
            try
            {
                _dataStore?.Delete(entry);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on Delete");
                Console.WriteLine(e);
                throw;
            }
        }

        public PhonebookEntry Put(PhonebookEntry entry)
        {
            try
            {
                return _dataStore?.Put(entry);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on Put");
                Console.WriteLine(e);
                throw;
            }
        }

        public PhonebookEntry Post(PhonebookEntry entry)
        {
            try
            {
                return _dataStore?.Post(entry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void CleanDb()
        {
            try
            {
                _dataStore?.CleanDb();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on CleanDb");
                Console.WriteLine(e);
                throw;
            }
        }
    }
}