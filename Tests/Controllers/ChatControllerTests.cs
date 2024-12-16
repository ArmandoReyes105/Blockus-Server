using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Controllers;
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
    public class ChatControllerTests
    {
        private ChatController _controller;
        private Dictionary<string, IChatServiceCallback> _mockUsersInChat;
        private Dictionary<string, ActiveMatch> _mockActiveMatches;

        [TestInitialize]
        public void SetUp()
        {
            _mockUsersInChat = new Dictionary<string, IChatServiceCallback>();
            _mockActiveMatches = new Dictionary<string, ActiveMatch>();
            _controller = new ChatController(_mockUsersInChat, _mockActiveMatches);
        }

        [TestMethod]
        public void JoinToChat_ShouldAddUserToChat()
        {
            // Arrange
            var username = "TestUser";
            var callbackMock = new Mock<IChatServiceCallback>();

            // Act
            _controller.JoinToChat(username, callbackMock.Object);

            // Assert
            Assert.IsTrue(_mockUsersInChat.ContainsKey(username));
            Assert.AreEqual(callbackMock.Object, _mockUsersInChat[username]);
        }

        [TestMethod]
        public void SendMessage_ShouldSendMessageToOtherPlayersInMatch()
        {
            // Arrange
            var senderUsername = "SenderUser";
            var receiverUsername = "ReceiverUser";
            var matchCode = "Match123";
            var message = "Hello!";

            var senderCallbackMock = new Mock<IChatServiceCallback>();
            var receiverCallbackMock = new Mock<IChatServiceCallback>();

            _mockUsersInChat[senderUsername] = senderCallbackMock.Object;
            _mockUsersInChat[receiverUsername] = receiverCallbackMock.Object;

            var match = new ActiveMatch
            {
                Players = new Dictionary<Color, PlayerState>
                {
                    { Color.Red, new PlayerState { Username = senderUsername } },
                    { Color.Blue, new PlayerState { Username = receiverUsername } }
                }
            };

            _mockActiveMatches[matchCode] = match;

            // Act
            _controller.SendMessage(senderUsername, matchCode, message);

            // Assert
            receiverCallbackMock.Verify(c => c.OnReciveMessage(senderUsername, message), Times.Once);
            senderCallbackMock.Verify(c => c.OnReciveMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void SendMessage_ShouldThrowKeyNotFoundException_WhenMatchCodeInvalid()
        {
            // Arrange
            var senderUsername = "TestUser";
            var matchCode = "InvalidMatch";
            var message = "Message";

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() =>
                _controller.SendMessage(senderUsername, matchCode, message));
        }

        [TestMethod]
        public void SendMessage_ShouldThrowKeyNotFoundException_WhenUserNotInChat()
        {
            // Arrange
            var senderUsername = "TestUser";
            var matchCode = "Match123";
            var message = "Message";

            var match = new ActiveMatch
            {
                Players = new Dictionary<Color, PlayerState>
                {
                    { Color.Red, new PlayerState { Username = "OtherUser" } }
                }
            };

            _mockActiveMatches[matchCode] = match;

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() =>
                _controller.SendMessage(senderUsername, matchCode, message));
        }

        [TestMethod]
        public void LeaveChat_ShouldRemoveUserFromChat()
        {
            // Arrange
            var username = "TestUser";
            var callbackMock = new Mock<IChatServiceCallback>();
            _mockUsersInChat[username] = callbackMock.Object;

            // Act
            _controller.LeaveChat(username);

            // Assert
            Assert.IsFalse(_mockUsersInChat.ContainsKey(username));
        }

        
    }
}
