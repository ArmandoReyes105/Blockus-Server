using Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Services.Utilities
{
    public static class RandomGenerator
    {

        public static string GenerateRandomCode()
        {
            StringBuilder code = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 3; i++)
            {
                char letter = (char)random.Next('A', 'Z' + 1);
                code.Append(letter);
            }

            for (int i = 0; i < 3; i++)
            {
                int number = random.Next(0, 10);
                code.Append(number);
            }

            return code.ToString();
        }

        public static Color GetColorNotInList(IEnumerable<Color> excludeColors)
        {

            var allColors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();
            var availableColors = allColors.Except(excludeColors).ToList();

            var random = new Random();
            var randomColor = availableColors[random.Next(availableColors.Count)];
            
            return randomColor; 
        }

        public static int GetRandomTurn(int numberOfPlayer, List<int> usedTurns)
        {
            var availableTurns = Enumerable.Range(1, numberOfPlayer).Except(usedTurns).ToList();
            var random = new Random();

            return availableTurns[random.Next(availableTurns.Count)]; 
        }

        public static List<Color> ShuffleColors(List<Color> colors)
        {
            Random random = new Random();
            return colors.OrderBy(_ => random.Next()).ToList();
        }

    }
}
