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

        public int Time { get; set; }
        
        public Dictionary<Color, PlayerState> Players { get; set; } = new Dictionary<Color, PlayerState>();

        public Color GetCurrentColorTurn()
        {
            return TurnOrder[Turn]; 
        }
    }
}
