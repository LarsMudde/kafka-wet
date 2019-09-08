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
    public class SubscriptionController : ControllerBase
    {

        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionsService)
        {
            _subscriptionService = subscriptionsService;
        }

        [HttpPost]
        public async Task<IActionResult> PostSubscription([FromBody] Subscription subscription)
        {
            await this._subscriptionService.PublishSubscriptionAsync(subscription);

            return Ok();
        }

    }
}
