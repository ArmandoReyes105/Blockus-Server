using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Controllers;
using Services.Dtos;
using Services.Enums;
using Services.Interfaces;
using Services.MatchState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Controllers
{
    [TestClass]
    public class MatchControllerTests
    {
        private MatchController _controller;
        private Dictionary<string, MatchDTO> _mockMatches;
        private Dictionary<string, ActiveMatch> _mockActiveMatches;
        private Dictionary<string, IMatchMakingServiceCallback> _mockUsersInMatchMaking;
        private Dictionary<string, IMatchServiceCallback> _mockUsersInActiveMatch;
        private Dictionary<string, MatchResumeDTO> _mockMatchResumes;

        [TestInitialize]
        public void SetUp()
        {
            _mockMatches = new Dictionary<string, MatchDTO>();
            _mockActiveMatches = new Dictionary<string, ActiveMatch>();
            _mockUsersInMatchMaking = new Dictionary<string, IMatchMakingServiceCallback>();
            _mockUsersInActiveMatch = new Dictionary<string, IMatchServiceCallback>();
            _mockMatchResumes = new Dictionary<string, MatchResumeDTO>();

            _controller = new MatchController(
                _mockUsersInMatchMaking,
                _mockUsersInActiveMatch,
                _mockMatches,
                _mockActiveMatches,
                _mockMatchResumes);
        }

        [TestMethod]
        public void JoinToActiveMatch_ShouldInitializeActiveMatch_WhenMatchDoesNotExist()
        {
            // Arrange
            var username = "Player1";
            var callbackMock = new Mock<IMatchServiceCallback>();

            var match = new MatchDTO
            {
                MatchCode = "123ABC",
                NumberOfPlayers = 2,
                Players = new Dictionary<Color, PublicAccountDTO>
                {
                    { Color.Red, new PublicAccountDTO { Username = username } },
                    { Color.Green, new PublicAccountDTO { Username = "Player2" } }
                }
            };

            _mockMatches[match.MatchCode] = match;

            // Act
            var result = _controller.JoinToActiveMatch(username, callbackMock.Object);

            // Assert
            Assert.IsTrue(_mockActiveMatches.ContainsKey(match.MatchCode));
            Assert.AreEqual(match.MatchCode, result.MatchCode);
            Assert.IsTrue(_mockUsersInActiveMatch.ContainsKey(username));
        }

        [TestMethod]
        public void GetCurrentBlock_ShouldReturnCurrentBlockOfMatch()
        {
            // Arrange
            var matchCode = "123ABC";
            var expectedBlock = new BlockDTO { Block = Block.Block01 };

            var activeMatch = new ActiveMatch
            {
                MatchCode = matchCode,
                Block = expectedBlock.Block,
                Players = new Dictionary<Color, PlayerState>(),
                Turn = 0,
                TurnOrder = new List<Color> { Color.Red }
            };

            _mockActiveMatches[matchCode] = activeMatch;

            // Act
            Console.WriteLine(_mockActiveMatches[matchCode].Block.ToString());
            
            var result = _controller.GetCurrentBlock(matchCode);
            
            // Assert
            Assert.AreEqual(Block.Block01, result.Block);
        }

        [TestMethod]
        public void PlaceBlock_ShouldAdvanceTurn_WhenBlockPlacedSuccessfully()
        {
            // Arrange
            var matchCode = "123ABC";
            var playerUsername = "Player1";

            var activeMatch = new ActiveMatch
            {
                MatchCode = matchCode,
                Block = Block.Block01,
                Players = new Dictionary<Color, PlayerState>
                {
                    { Color.Red, new PlayerState { Username = playerUsername, Color = Color.Red, BlocksList = new List<Block> { Block.Block01, Block.Block02 } } },
                    { Color.Green, new PlayerState { Username = "Player2", Color = Color.Green, BlocksList = new List<Block> { Block.Block02, Block.Block03 } } }
                },
                Turn = 0,
                TurnOrder = new List<Color> { Color.Red, Color.Green }
            };

            _mockActiveMatches[matchCode] = activeMatch;
            var callbackMock = new Mock<IMatchServiceCallback>();
            _mockUsersInActiveMatch[playerUsername] = callbackMock.Object;
            _mockUsersInActiveMatch["Player2"] = callbackMock.Object;

            // Act
            var result = _controller.PlaceBlock(matchCode, 1);

            // Assert
            Assert.AreEqual(GameResult.None, result);
            Assert.AreEqual(1, activeMatch.Turn);
            Assert.AreEqual(1, activeMatch.Players[Color.Red].Puntuation);
            callbackMock.Verify(c => c.OnBlockPlaced(), Times.Once);
        }

        [TestMethod]
        public void SkipTurn_ShouldAdvanceTurn_WhenNotAllPlayersSkipped()
        {
            // Arrange
            var matchCode = "123ABC";
            var activeMatch = new ActiveMatch
            {
                MatchCode = matchCode,
                Players = new Dictionary<Color, PlayerState>
                {
                    { Color.Red, new PlayerState { Username = "Player1", Color = Color.Red } },
                    { Color.Green, new PlayerState { Username = "Player2", Color = Color.Green } }
                },
                Turn = 0,
                TurnOrder = new List<Color> { Color.Red, Color.Green },
                Skips = 0
            };

            _mockActiveMatches[matchCode] = activeMatch;
            var callbackMock = new Mock<IMatchServiceCallback>();
            _mockUsersInActiveMatch["Player1"] = callbackMock.Object;
            _mockUsersInActiveMatch["Player2"] = callbackMock.Object;

            // Act

            var result = _controller.SkipTurn(matchCode);

            // Assert
            Assert.AreEqual(GameResult.None, result);
            Assert.AreEqual(1, activeMatch.Turn);
            Assert.AreEqual(1, activeMatch.Skips);
        }

        [TestMethod]
        public void LeaveActiveMatch_ShouldRemovePlayerFromMatch()
        {
            // Arrange
            var matchCode = "123ABC";
            var username = "Player1";

            var activeMatch = new ActiveMatch
            {
                MatchCode = matchCode,
                Players = new Dictionary<Color, PlayerState>
                {
                    { Color.Red, new PlayerState { Username = username, Color = Color.Red } },
                    { Color.Green, new PlayerState { Username = "Player2", Color = Color.Green } }
                },
                Turn = 0,
                TurnOrder = new List<Color> { Color.Red, Color.Green }
            };

            _mockActiveMatches[matchCode] = activeMatch;
            var callbackMock = new Mock<IMatchServiceCallback>();
            _mockUsersInActiveMatch[username] = callbackMock.Object;
            _mockUsersInActiveMatch["Player2"] = callbackMock.Object;

            // Act
            _controller.LeaveActiveMatch(username);

            // Assert
            Assert.IsFalse(activeMatch.Players.ContainsKey(Color.Red));
            Assert.IsFalse(_mockUsersInActiveMatch.ContainsKey(username));
        }
    }
}
