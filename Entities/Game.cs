using System;

namespace Entities
{
    public class Game
    {
        public int Id { get; set; }
        public int IdGameType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enabled { get; set; }
        public int? WinningNumber { get; set; }
        public string WinningColor { get; set; }
        public virtual GameType GameType { get; set; }
    }
}
