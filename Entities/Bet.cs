using System;

namespace Entities
{
    public class Bet
    {
        public int Id { get; set; }
        public int IdGame { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        public double MoneyBet { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsWinner { get; set; }
        public double EarnedMoney { get; set; }
        public virtual Game Game { get; set; }

    }
}
