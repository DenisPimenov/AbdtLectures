using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.CardService.Contracts;

namespace Practice5.CardService.Controllers
{
    public class CardController : ControllerBase
    {
        private readonly CardContext _ctx;

        public CardController(CardContext ctx)
        {
            _ctx = ctx;
        }

        [Obsolete]
        [HttpGet("card")]
        public async Task<ActionResult<IEnumerable<CardResponseSimple>>> GetCardSimpleV1(long userId, string[] tags)
        {
            var cards = await _ctx.Cards.Include(c => c.Tags).Where(c =>
                    c.UserId == userId && c.Tags.Any(t=>tags.Contains(t.Value)))
                .ToListAsync();
            return Ok(cards.Select(c =>
            {
                var mask = c.Pan.Remove(4, 6).Insert(4, "******");
                return new CardResponseSimple(mask, c.Balance);
            }));
        }

        [HttpGet("v2/card")]
        public async Task<ActionResult<CardResponseSimple>> GetCardSimpleV2(long userId, string[] tags)
        {
            var cards = await _ctx.Cards.Include(c => c.Tags).Where(c =>
                    c.UserId == userId && c.Tags.Any(t=>tags.Contains(t.Value)))
                .ToListAsync();
            return Ok(cards.Select(c =>
            {
                var mask = c.Pan.Remove(4, 6).Insert(4, "******");
                return new CardResponseSimple(mask, c.Balance);
            }));
        }
    }
}