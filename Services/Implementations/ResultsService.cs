using Data.Dao;
using Services.Dtos;
using Services.Enums;
using Services.Interfaces;
using System;

namespace Services.Implementations
{
    public partial class ServiceImplementation : IResultsService
    {
        public MatchResumeDTO GetMatchResume(string matchCode)
        {
            var matchResume = matchResumes[matchCode];

            matchResume.Count++; 

            if (matchResume.Count == matchResume.PlayerList.Count)
            {
                matchResumes.Remove(matchCode);
                activeMatches.Remove(matchCode);

                foreach (var player in matchResume.PlayerList)
                {
                    _usersInActiveMatch.Remove(player.Username);
                    _usersInChat.Remove(player.Username);
                }

                Console.WriteLine("Ya se elimino completamente la partida: " + matchCode);
            }

            return matchResume; 
        }

        public int UpdateResults(int idAccount, GameResult result)
        {
            AccountDao dao = new AccountDao();
            int operationResult;

            if (GameResult.Winner == result)
            {
                operationResult = dao.IncreaseVictories(idAccount);
            }
            else
            {
                operationResult = dao.IncreaseLosses(idAccount);
            }

            return operationResult;
        }
    }
}
