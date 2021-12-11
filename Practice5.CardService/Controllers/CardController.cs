using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Practice5.CardService.Controllers
{
    public record CardResponseSimple(string SafePan, decimal Balance);

    [Route("card")]
    public class CardController : ControllerBase
    {
        private readonly CardContext _ctx;

        public CardController(CardContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<ActionResult<CardResponseSimple>> GetCardSimple(long userId, string[] tags)
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