using System.Collections.Generic;

namespace Practice5.CardService.Models
{
    public class Card
    {
        public long Id { get; set; }

        public string Pan { get; set; }

        public Expire Expire { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public long UserId { get; set; }

        public decimal Balance { get; set; }
    }

    public class Tag
    {
        public string Value { get; set; }
    }
}