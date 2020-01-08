using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhonebookApi.DTO;

namespace PhonebookApi.Controllers
{
    /// <summary>
    /// Controller to manage phonebook entries
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [EnableCors(Startup.AllowSpecificOrigins)]
    public class ContactsController
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IPhonebookDataStore _dataStore;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataStore"></param>
        public ContactsController(IPhonebookDataStore dataStore, ILogger<ContactsController> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }

        /// <summary>
        /// Add a new entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromBody] PhonebookEntry entry)
        {
            try
            {
                var result = _dataStore.Post(entry);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, "Error on Post", entry);
                throw;
            }
        }

        /// <summary>
        /// Update an entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>ActionResult with PhonebookEntry</returns>
        [HttpPut]
        public ActionResult<PhonebookEntry> Put([FromBody] PhonebookEntry entry)
        {
            try
            {
                var result = _dataStore.Put(entry);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on Put", entry);
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get all entries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<PhonebookEntry>> Get()
        {
            try
            {
                var items = _dataStore.GetAll();
                return items;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on Get");
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Delete an entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var item = Get(id);
                if (item.Value != null)
                {
                    _dataStore.Delete(item.Value);
                }
                else
                {
                    return new NotFoundResult();
                }

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on Delete", id);
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get a specific entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<PhonebookEntry> Get(int id)
        {
            try
            {
                var getResult = _dataStore.Get(id);

                return getResult != null ? new ActionResult<PhonebookEntry>(getResult) : new NotFoundResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on Get", id);
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Clean the DB
        /// Testing method, not exposed
        /// </summary>
        [NonAction]
        public void CleanDb()
        {
            _dataStore.CleanDb();
        }
    }
}