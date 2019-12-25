using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhonebookApi.DTO;
using Unity;

namespace PhonebookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IPhonebookDataStore _dataStore; 
        
        public ContactsController(IPhonebookDataStore dataStore)
        {
            _dataStore = dataStore;
        }
        
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public ActionResult Post([Microsoft.AspNetCore.Mvc.FromBody]PhonebookEntry entry)
        {
            _dataStore.Post(entry);
            return new OkResult();
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public ActionResult Put([Microsoft.AspNetCore.Mvc.FromBody] PhonebookEntry entry)
        {
            _dataStore.Put(entry);
            return new OkResult();
        }    

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public List<PhonebookEntry> Get()
        {
            return _dataStore.GetAll();
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var item = Get(id);
            _dataStore.Delete(item);
            return new OkResult();
        }
        
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public PhonebookEntry Get(int id)
        {
            return _dataStore.Get(id);
        }

        [Microsoft.AspNetCore.Mvc.NonAction]
        public void CleanDb()
        {
            _dataStore.CleanDb();
        }
    }
} 