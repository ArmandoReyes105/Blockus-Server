using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Controllers;
using Services.Enums; 
using Services.Dtos;
using Services.Interfaces;
using System.Collections.Generic;

namespace Tests.Controllers
{
    [TestClass]
    public class MatchMakingControllerTests
    {

        private MatchMakingController _controller;
        private Dictionary<string, IMatchMakingServiceCallback> _mockUsersInMatchMaking;
        private Dictionary<string, MatchDTO> _mockMatches;

        [TestInitialize]
        public void SetUp()
        {
            _mockUsersInMatchMaking = new Dictionary<string, IMatchMakingServiceCallback>();
            _mockMatches = new Dictionary<string, MatchDTO>();
            _controller = new MatchMakingController(_mockUsersInMatchMaking, _mockMatches); 
        }

        [TestMethod]
        public void CreateMatch_ShouldAddUserAndMatchToDictionaries()
        {
            var mockCallback = new Mock<IMatchMakingServiceCallback>();
            var hostAccount = new PublicAccountDTO { Username = "HostUser" };

            var result = _controller.CreateMatch(hostAccount, mockCallback.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual("HostUser", result.Host);
            Assert.IsTrue(_mockMatches.ContainsKey(result.MatchCode));
        }

        [TestMethod]
        public void JoinToMatch_ShouldAddPlayerToMatch_WhenMatchExistsAndHasSpace()
        {
            var mockCallback = new Mock<IMatchMakingServiceCallback>();
            var newAccount = new PublicAccountDTO { Username = "NewUser" };

            var match = new MatchDTO
            {
                MatchCode = "123ABC",
                NumberOfPlayers = 4,
                Players = new Dictionary<Color, PublicAccountDTO>()
            };

            _mockMatches.Add(match.MatchCode, match);
            
            var result = _controller.JoinToMatch(newAccount, "123ABC", mockCallback.Object);

            Assert.AreEqual("123ABC", result.MatchCode);
            Assert.IsTrue(_mockUsersInMatchMaking.ContainsKey("NewUser")); 
        }

        [TestMethod]
        public void JoinToMatch_ShouldReturnError_WhenMatchDoesNotExist()
        {
            //Arrange
            var mockCallback = new Mock<IMatchMakingServiceCallback>();
            var newAccount = new PublicAccountDTO { Username = "NewUser" };

            //Act
            var result = _controller.JoinToMatch(newAccount, "123ABC", mockCallback.Object);

            //Assert
            Assert.AreEqual("0", result.Host);
            Assert.IsFalse(_mockUsersInMatchMaking.ContainsKey("NewUser"));  
        }

        [TestMethod]
        public void JoinToMatch_ShouldReturnError_WhenMatchIsFull()
        {
            //Arrange
            var hostCallback = new Mock<IMatchMakingServiceCallback>();
            var hostAccount = new PublicAccountDTO { Username = "HostUser" };

            var match = _controller.CreateMatch(hostAccount, hostCallback.Object);

            for (int i = 0; i < 3; i++)
            {
                var playerAccount = new PublicAccountDTO { Username = $"playerUser{i}" };
                var playerCallback = new Mock<IMatchMakingServiceCallback>();
                _controller.JoinToMatch(playerAccount, match.MatchCode, playerCallback.Object);
            }

            var numberOfPlayers = match.Players.Count;
            var newUser = new PublicAccountDTO { Username = "NewUser" };
            var newUserCallback = new Mock<IMatchMakingServiceCallback>();

            //Act
            var result = _controller.JoinToMatch(newUser, match.MatchCode, newUserCallback.Object);

            //Assert
            Assert.AreEqual("-1", result.Host);
            Assert.AreEqual(numberOfPlayers, match.Players.Count);
            hostCallback.Verify(c => c.NotifyPlayerEntry(It.IsAny<MatchDTO>()), Times.Exactly(3));
        }

        [TestMethod]
        public void LeaveMatch_HostLeave_RemoveMatchPlayerAndNotifyPlayers()
        {
            //Arrange
            var mockCallback1 = new Mock<IMatchMakingServiceCallback>();
            var mockCallback2 = new Mock<IMatchMakingServiceCallback>();

            var match = new MatchDTO
            {
                MatchCode = "567ABC",
                Host = "HostUser",
                Players = new Dictionary<Color, PublicAccountDTO>
                {
                    { Color.Red, new PublicAccountDTO { Username = "HostUser" } },
                    { Color.Green, new PublicAccountDTO { Username = "Player2" } }
                }
            };

            _mockMatches.Add(match.MatchCode, match);
            _mockUsersInMatchMaking.Add("HostUser", mockCallback1.Object);
            _mockUsersInMatchMaking.Add("Player2", mockCallback2.Object);

            //Act
            _controller.LeaveMatch("HostUser");

            //Assert
            Assert.IsFalse(_mockUsersInMatchMaking.ContainsKey("HostUser"));
            Assert.IsFalse(_mockMatches.ContainsKey(match.MatchCode));
            mockCallback2.Verify(m => m.NotifyHostExit(It.IsAny<MatchDTO>()), Times.Once);
        }

        [TestMethod]
        public void LeaveMatch_PlayerLeave_NotifyPlayersAndRemoveUserFromMatch()
        {
            var hostCallback = new Mock<IMatchMakingServiceCallback>();
            var playerCallback = new Mock<IMatchMakingServiceCallback>();

            var match = new MatchDTO
            {
                MatchCode = "123ABC",
                Host = "HostUser",
                Players = new Dictionary<Color, PublicAccountDTO>
                {
                    { Color.Red, new PublicAccountDTO { Username = "HostUser" } },
                    { Color.Green, new PublicAccountDTO { Username = "Player2" } }
                }
            };

            _mockMatches.Add(match.MatchCode, match);
            _mockUsersInMatchMaking.Add("HostUser", hostCallback.Object);
            _mockUsersInMatchMaking.Add("Player2", playerCallback.Object);

            //Act
            _controller.LeaveMatch("Player2");

            //Assert
            Assert.IsFalse(_mockUsersInMatchMaking.ContainsKey("Player2"));
            Assert.AreEqual(1, match.Players.Count);
            hostCallback.Verify(c => c.NotifyPlayerExit(It.IsAny<MatchDTO>()), Times.Once);
        }

    }
}
