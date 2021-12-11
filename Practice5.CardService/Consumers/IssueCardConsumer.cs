using System;
using System.Threading.Tasks;
using MassTransit;
using Practice5.CardService.Models;
using Shared.Messages;

namespace Practice5.CardService.Consumers
{
    public class IssueCardConsumer : IConsumer<IssueSuperCard>
    {
        private static Random rnd = new Random(123);
        private readonly CardContext _db;

        public IssueCardConsumer(CardContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<IssueSuperCard> context)
        {
            Console.WriteLine("new message");
            await Task.Delay(5000);
            _db.Cards.Add(new Card()
            {
                Balance = rnd.Next(10, 10000),
                Expire = new Expire()
                {
                    Month = 10,
                    Year = 2025
                },
                Pan = "1234123412341234",
                UserId = context.Message.OwnerId,
                Tags = new Tag[]
                {
                    new() { Value = "virtual" },
                    new() { Value = context.Message.Partner }
                }
            });
            await _db.SaveChangesAsync();
        }
    }
}