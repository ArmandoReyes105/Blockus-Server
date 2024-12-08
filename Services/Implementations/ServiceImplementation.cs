using Services.Controllers;
using Services.Dtos;
using Services.Interfaces;
using Services.MatchState;
using System.Collections.Generic;

namespace Services.Implementations
{
    public partial class ServiceImplementation
    {

        private static readonly Dictionary<string, IMatchMakingServiceCallback> _usersInMatchMaking = new Dictionary<string, IMatchMakingServiceCallback>();
        private static readonly Dictionary<string, IMatchServiceCallback> _usersInActiveMatch = new Dictionary<string, IMatchServiceCallback>();
        private static readonly Dictionary<string, IChatServiceCallback> _usersInChat = new Dictionary<string, IChatServiceCallback>();

        private static readonly Dictionary<string, MatchDTO> matches = new Dictionary<string, MatchDTO>();
        private static readonly Dictionary<string, ActiveMatch> activeMatches = new Dictionary<string, ActiveMatch>();
        private static readonly Dictionary<string, MatchResumeDTO> matchResumes = new Dictionary<string, MatchResumeDTO>();

        private readonly MatchMakingController _matchMakingController;
        private readonly MatchController _matchController;
        private readonly ChatController _chatController; 

        public ServiceImplementation()
        {
            _matchMakingController = new MatchMakingController(_usersInMatchMaking, matches);
            _matchController = new MatchController(_usersInMatchMaking, _usersInActiveMatch, matches, activeMatches, matchResumes);
            _chatController = new ChatController(_usersInChat, activeMatches);
        }
    }
}
