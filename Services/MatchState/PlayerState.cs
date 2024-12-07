using Services.Enums;
using System;
using System.Collections.Generic;

namespace Services.MatchState
{
    public class PlayerState
    {
        public string Username { get; set; }

        public Color Color { get; set; }

        public int Puntuation { get; set; } = 0; 

        public List<Block> BlocksList { get; set; } = new List<Block>
        {
            Block.Block01,
            Block.Block02,
            Block.Block03,
        };

        public Block GetRandomBlock()
        {
            Random random = new Random();
            int index = random.Next(BlocksList.Count);
            return BlocksList[index];
        }

        public void RemoveBlock(Block block)
        {
            BlocksList.Remove(block);
        }
    }
}
