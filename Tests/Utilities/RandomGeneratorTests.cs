using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Enums;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Utilities
{
    [TestClass]
    public class RandomGeneratorTests
    {
        [TestMethod]
        public void GenerateRandomCode_ShouldReturnCodeWithLength6()
        {
            var result = RandomGenerator.GenerateRandomCode();

            Assert.AreEqual(6, result.Length); 
        }

        [TestMethod]
        public void GenerateRandomCode_ShouldContainOnlyUpperCaseLettersAndNumbers()
        {
            var result = RandomGenerator.GenerateRandomCode();

            var charString = result.Substring(0,3);
            var numString = result.Substring(3, 3);

            Assert.IsTrue(charString.All(c => char.IsUpper(c)));
            Assert.IsTrue(numString.All(c => char.IsDigit(c)));
        }

        [TestMethod]
        public void GetColorNotInList_ShouldReturnValidColor()
        {
            var result = RandomGenerator.GetColorNotInList(Enumerable.Empty<Color>());

            Assert.IsTrue(Enum.IsDefined(typeof(Color), result)); 
        }

        [TestMethod]
        public void GetColorNotInList_ShouldReturnColorNotInExcludedList()
        {
            var excludeColors = new List<Color> { Color.Yellow , Color.Green };

            var result = RandomGenerator.GetColorNotInList(excludeColors);

            Assert.IsFalse(excludeColors.Contains(result));
        }

        [TestMethod]
        public void GetColorNotInList_ShouldReturnRedColor()
        {
            var allColorsExceptRed = new List<Color> { Color.Blue, Color.Green, Color.Yellow };

            var result = RandomGenerator.GetColorNotInList(allColorsExceptRed);

            Assert.AreEqual(Color.Red, result);
        }
    }
}
