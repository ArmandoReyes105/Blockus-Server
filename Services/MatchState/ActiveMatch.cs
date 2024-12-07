using Services.Dtos;
using Services.Enums;
using System.Collections.Generic;

namespace Services.MatchState
{
    public class ActiveMatch
    {
        public int Turn { get; set; } = 0; 

        public List<Color> TurnOrder { get; set; } = new List<Color>();

        public int Skips { get; set; } = 0;

        public Block Block { get; set; }
        
        public Dictionary<Color, PlayerState> Players { get; set; } = new Dictionary<Color, PlayerState>();

        public PlayerState GetCurrentPlayer()
        {
            Color currentColorTurn = TurnOrder[Turn];
            return Players[currentColorTurn];
        }

        public PlayerState GetNextPlayer()
        {
            int nextTurn = GetNextTurn();
            Color nextColorTurn = TurnOrder[nextTurn];
            return Players[nextColorTurn];
        }

        public BlockDTO GetCurrentBlockAsDTO()
        {
            var block = new BlockDTO { Block = Block, Color = TurnOrder[Turn] };
            return block;
        }

        public void AdvanceTurn() => Turn = GetNextTurn(); 

        private int GetNextTurn() => (Turn + 1) % Players.Count;
    }
}
