using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhonebookApi.DTO;
using Unity;

namespace PhonebookApi.Controllers
{
    /// <summary>
    /// Controller to manage phonebook entries
    /// </summary>
    [ApiController]
    [Route("[controller]")]
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
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public ActionResult Post([Microsoft.AspNetCore.Mvc.FromBody]PhonebookEntry entry)
        {
            _dataStore.Post(entry);
            return new OkResult();
        }

        /// <summary>
        /// Update an entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public ActionResult Put([Microsoft.AspNetCore.Mvc.FromBody] PhonebookEntry entry)
        {
            _dataStore.Put(entry);
            return new OkResult();
        }    

        /// <summary>
        /// Get all entries
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public List<PhonebookEntry> Get()
        {
            return _dataStore.GetAll();
        }

        /// <summary>
        /// Delete an entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var item = Get(id);
            _dataStore.Delete(item);
            return new OkResult();
        }
        
        /// <summary>
        /// Get a specific entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public PhonebookEntry Get(int id)
        {
            return _dataStore.Get(id);
        }

        /// <summary>
        /// Clean the DB
        /// Testing method, not exposed
        /// </summary>
        [Microsoft.AspNetCore.Mvc.NonAction]
        public void CleanDb()
        {
            _dataStore.CleanDb();
        }
    }
} 