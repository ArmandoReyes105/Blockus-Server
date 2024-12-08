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
