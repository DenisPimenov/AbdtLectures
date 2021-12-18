using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Practice5.ApiGateway.Controllers;
using Practice5.CardService.Contracts;

namespace Practice5.ApiGateway.Apis
{
    public interface ICardApiClient
    {
        Task<IEnumerable<CardModel>> GetPartnerCards(long userId, string partner);
    }

    public class CardApiClient : ICardApiClient
    {
        private readonly HttpClient _client;

        public CardApiClient(HttpClient client) => _client = client;

        public async Task<IEnumerable<CardModel>> GetPartnerCards(long userId, string partner)
        {
            var cards = await _client.GetFromJsonAsync<IEnumerable<CardResponseSimple>>(
                $"/card?userId={userId}&tags={partner}");
            return cards?.Select(c=> new CardModel(c.SafePan,c.Balance)) ?? Array.Empty<CardModel>();
        }
    }
}