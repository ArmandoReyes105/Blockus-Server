using Services.Interfaces;
using Services.MatchState;
using System.Collections.Generic;
using System; 

namespace Services.Controllers
{
    public class ChatController
    {

        private readonly Dictionary<string, IChatServiceCallback> _usersInChat;
        private readonly Dictionary<string, ActiveMatch> _activeMatches;

        public ChatController(
            Dictionary<string, IChatServiceCallback> usersInChat,
            Dictionary<string, ActiveMatch> activeMatches)
        {
            _usersInChat = usersInChat;
            _activeMatches = activeMatches;
        }

        public void JoinToChat(string username, IChatServiceCallback callback)
        {
            _usersInChat[username] = callback;
        }

        public void SendMessage(string username, string matchCode, string message)
        {
            var activeMatch = _activeMatches[matchCode];

            foreach (var player in activeMatch.Players.Values)
            {
                if (player.Username != username)
                {
                    _usersInChat[player.Username].OnReciveMessage(username, message);
                }
            }
        }

        public void LeaveChat(string username)
        {
            _usersInChat.Remove(username);
        }
    }
}
