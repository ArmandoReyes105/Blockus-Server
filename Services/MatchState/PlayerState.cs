using Services.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Services.MatchState
{
    [DataContract]
    public class PlayerState
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public Color Color { get; set; }

        [DataMember]
        public int Puntuation { get; set; } = 0;

        [DataMember]
        public List<Block> BlocksList { get; set; } = new List<Block>
        {
            Block.Block01,
            Block.Block02,
            Block.Block03,
            Block.Block04,
            Block.Block05,
            Block.Block06,
            Block.Block07,
            Block.Block08,
            Block.Block09,
            Block.Block10,
            Block.Block11,
            Block.Block12,
            Block.Block13,
            Block.Block14,
            Block.Block15,
            Block.Block16,
            Block.Block17,
            Block.Block18,
            Block.Block19,
            Block.Block20,
            Block.Block21
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
