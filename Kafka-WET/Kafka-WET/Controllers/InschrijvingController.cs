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

        [HttpPost]
        public async Task<IActionResult> PostInschrijving([FromBody] Inschrijving inschrijving)
        {
            await this._inschrijvingService.PublishInschrijvingAsync(inschrijving);

            return Ok();
        }

    }
}
