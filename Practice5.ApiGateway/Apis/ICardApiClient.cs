using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Practice5.ApiGateway.Controllers;

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
            var cards = await _client.GetFromJsonAsync<IEnumerable<CardModel>>($"/card?userId={userId}&tags={partner}");
            return cards ?? Array.Empty<CardModel>();
        }
    }
}