using Services.Dtos;
using Services.Enums;
using Services.Interfaces;
using Services.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Services.Implementations
{
    public partial class ServiceImplementation : IMatchMakingService
    {
        private static readonly Dictionary<string, IMatchMakingServiceCallback> usersInMatchMaking = new Dictionary<string, IMatchMakingServiceCallback> ();
        private static readonly Dictionary<string, MatchDTO> matches = new Dictionary<string, MatchDTO>();

        public MatchDTO CreateMatch(PublicAccountDTO hostAccount)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IMatchMakingServiceCallback>();
            var matchCode = RandomGenerator.GenerateRandomCode();
            var color = RandomGenerator.GetColorNotInList(Enumerable.Empty<Color>());

            var match = new MatchDTO
            {
                MatchType = GameType.Private,
                MatchCode = matchCode,
                Host = hostAccount.Username,
                NumberOfPlayers = 4,
                Players = new Dictionary<Color, PublicAccountDTO> { [color] = hostAccount }
            };

            usersInMatchMaking.Add(hostAccount.Username, callback);
            matches.Add(match.MatchCode, match); 

            return match; 
        }

        public MatchDTO JoinToMatch(PublicAccountDTO account, string matchCode)
        {
            
            if (!matches.TryGetValue(matchCode, out var match))
            {
                return new MatchDTO { Host = "0"}; 
            }

            if (match.Players.Count() >= match.NumberOfPlayers)
            {
                return new MatchDTO { Host = "-1" }; 
            }

            var callback = OperationContext.Current.GetCallbackChannel<IMatchMakingServiceCallback>();
            usersInMatchMaking.Add(account.Username, callback);

            var color = RandomGenerator.GetColorNotInList(match.Players.Keys);
            match.Players.Add(color,account);

            var players = match.Players.Values.Where(p => p.Username != account.Username).ToList();
            players.ForEach(player => usersInMatchMaking[player.Username].NotifyPlayerEntry(match)); 

            return match;
        }

        public void LeaveMatch(string username)
        {
            MatchDTO match = matches.Values.FirstOrDefault(m => m.Players.Values.Any(p => p.Username == username));
            usersInMatchMaking.Remove(username);

            if (match != null) 
            {
                var key = match.Players.FirstOrDefault(p => p.Value.Username == username).Key;
                match.Players.Remove(key);

                if (match.Host == username)
                {
                    
                    match.Players.Values.ToList().ForEach(p =>
                    {
                        usersInMatchMaking[p.Username].NotifyHostExit(match);
                        usersInMatchMaking.Remove(p.Username); 
                    });

                    matches.Remove(match.MatchCode);
                }
                else
                {
                    var players = match.Players.Values.ToList();
                    players.ForEach(p => usersInMatchMaking[p.Username].NotifyPlayerExit(match));
                }
            }
        }
    }
}
