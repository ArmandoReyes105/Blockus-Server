using Services.Dtos;
using Services.Enums;
using Services.Interfaces;
using Services.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Services.Controllers
{
    public class MatchMakingController
    {
        private readonly Dictionary<string, IMatchMakingServiceCallback> _usersInMatchMaking;
        private readonly Dictionary<string, MatchDTO> _matches;

        public MatchMakingController(Dictionary<string, IMatchMakingServiceCallback> usersInMatchMaking, Dictionary<string, MatchDTO> matches)
        {
            _usersInMatchMaking = usersInMatchMaking; 
            _matches = matches;
        }

        public MatchDTO CreateMatch(PublicAccountDTO hostAccount, IMatchMakingServiceCallback callback)
        {
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

            _usersInMatchMaking.Add(hostAccount.Username, callback);
            _matches.Add(match.MatchCode, match);

            return match;
        }

        public MatchDTO JoinToMatch(PublicAccountDTO account, string matchCode, IMatchMakingServiceCallback callback)
        {
            if (!_matches.TryGetValue(matchCode, out var match))
            {
                return new MatchDTO { Host = "0" };
            }

            if (match.Players.Count() >= match.NumberOfPlayers)
            {
                return new MatchDTO { Host = "-1" };
            }

            _usersInMatchMaking.Add(account.Username, callback);

            var color = RandomGenerator.GetColorNotInList(match.Players.Keys);
            match.Players.Add(color, account);

            var players = match.Players.Values.Where(p => p.Username != account.Username).ToList();
            players.ForEach(player => _usersInMatchMaking[player.Username].NotifyPlayerEntry(match));

            return match;
        }

        public void LeaveMatch(string username)
        {
            MatchDTO match = _matches.Values.FirstOrDefault(m => m.Players.Values.Any(p => p.Username == username));
            _usersInMatchMaking.Remove(username);

            if (match != null)
            {
                var key = match.Players.FirstOrDefault(p => p.Value.Username == username).Key;
                match.Players.Remove(key);

                if (match.Host == username)
                {

                    match.Players.Values.ToList().ForEach(p =>
                    {
                        _usersInMatchMaking[p.Username].NotifyHostExit(match);
                        _usersInMatchMaking.Remove(p.Username);
                    });

                    _matches.Remove(match.MatchCode);
                }
                else
                {
                    var players = match.Players.Values.ToList();
                    players.ForEach(p => _usersInMatchMaking[p.Username].NotifyPlayerExit(match));
                }
            }
        }
    }
}
