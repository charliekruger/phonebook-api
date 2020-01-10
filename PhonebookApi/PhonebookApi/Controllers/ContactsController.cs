using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhonebookApi.DTO;
using PhonebookApi.Interfaces;

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
        private readonly IPhonebookService _phonebookService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="phonebookService"></param>
        public ContactsController(IPhonebookService phonebookService, ILogger<ContactsController> logger)
        {
            _phonebookService = phonebookService;
            _logger = logger;
        }

        /// <summary>
        /// Add a new entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>OkResult</returns>
        /// <returns>BadRequestResult</returns>
        [HttpPost]
        public ActionResult Post([FromBody] PhonebookEntry entry)
        {
            try
            {
                if (entry == null)
                {
                    return new BadRequestResult();
                }
                
                var result = _phonebookService.Post(entry);
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
        /// <returns>BadRequestResult</returns>
        [HttpPut]
        public ActionResult<PhonebookEntry> Put([FromBody] PhonebookEntry entry)
        {
            try
            {
                if (entry == null)
                {
                    return new BadRequestResult();
                }
                
                var result = _phonebookService.Put(entry);
                return new OkObjectResult(result);
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
        /// <returns>OkResult with List of PhonebookEntry</returns>
        [HttpGet]
        public ActionResult<List<PhonebookEntry>> Get()
        {
            try
            {
                var items = _phonebookService.GetAll();
                return new OkObjectResult(items);
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
        /// <returns>OkResult</returns>
        /// <returns>NotFoundResult</returns>
        /// <returns>BadRequestResult</returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new BadRequestResult();
                }
                
                var item = Get(id);
                if (item.Value != null)
                {
                    _phonebookService.Delete(item.Value);
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
        /// <returns>ActionResult with PhonebookEntry</returns>
        /// <returns>BadRequestResult</returns>
        /// <returns>NotFoundResult</returns>
        [HttpGet("{id}")]
        public ActionResult<PhonebookEntry> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new BadRequestResult();
                }
                
                var getResult = _phonebookService.Get(id);

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
            _phonebookService.CleanDb();
        }
    }
}