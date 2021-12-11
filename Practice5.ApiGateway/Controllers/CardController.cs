using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Practice5.ApiGateway.Apis;
using Shared;
using Shared.Messages;

namespace Practice5.ApiGateway.Controllers
{
    public record CardModel(string SafePan, decimal Balance);

    [ApiController]
    [Route("card")]
    public class CardController : ControllerBase
    {
        private readonly Cache _cache;
        private readonly IBus _bus;
        private readonly ICardApiClient _cardApiClient;

        public CardController(Cache cache, IBus bus, ICardApiClient cardApiClient)
        {
            _cache = cache;
            _bus = bus;
            _cardApiClient = cardApiClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardModel>>> Get(long id, string partner)
        {
            var cards = await _cache.GetCollection<CardModel>(id.ToString());
            if (cards is { })
                return Ok(cards);

            cards = await _cardApiClient.GetPartnerCards(id, partner);
            return Ok(cards);
        }

        [HttpPost("issue")]
        public async Task<ActionResult<long>> Issue(long id, string partner)
        {
            await _bus.Publish(new IssueSuperCard {OwnerId = id, Partner = partner});
            return Ok(ApiResult.Ok());
        }
    }
}