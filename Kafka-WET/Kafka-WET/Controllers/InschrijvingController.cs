using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kafka_WET.Domain;
using Kafka_WET.Services.Service;
using Microsoft.AspNetCore.Mvc;

namespace Kafka_WET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InschrijvingController : ControllerBase
    {

        private readonly IInschrijvingService _inschrijvingService;

        public InschrijvingController(IInschrijvingService inschrijvingsService)
        {
            _inschrijvingService = inschrijvingsService;
        }

        [HttpPost("/inschrijving/{inschrijving}")]
        public async Task<IActionResult> PostInschrijving([FromBody] Inschrijving inschrijving)
        {
            await this._inschrijvingService.PublishInschrijvingAsync(inschrijving);

            return Ok();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
