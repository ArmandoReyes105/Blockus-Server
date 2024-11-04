using Services.Dtos;
using Services.Enums;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Services.Implementations
{
    public partial class ServiceImplementation : IMatchMakingService
    {
        private static Dictionary<string, IMatchMakingServiceCallback> usersInMatchMaking = new Dictionary<string, IMatchMakingServiceCallback> ();
        private static Dictionary<string, MatchDTO> matches = new Dictionary<string, MatchDTO>();

        public MatchDTO CreateMatch(PublicAccountDTO hostAccount)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IMatchMakingServiceCallback>();
            var match = new MatchDTO
            {
                MatchType = GameType.Private,
                ColorsOrder = GetRandomColorOrder(),
                MatchCode = GenerateRandomCode(),
                Host = hostAccount.Username,
                NumberOfPlayers = 4,
                Players = new List<PublicAccountDTO>()
            };

            match.Players.Add(hostAccount);

            usersInMatchMaking.Add(hostAccount.Username, callback);
            matches.Add(match.MatchCode, match); 

            return match; 
        }

        public bool JoinToMatch(PublicAccountDTO account, string matchCode)
        {
            return true; 
        }

        private IEnumerable<Color> GetRandomColorOrder()
        {
            var colors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();
            var random = new Random();
            return colors.OrderBy(c => random.Next()).ToList(); 
        }

        private string GenerateRandomCode()
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
    }
}
