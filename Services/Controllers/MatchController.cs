using Services.Dtos;
using Services.Enums;
using Services.Interfaces;
using Services.MatchState;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Controllers
{
    public class MatchController
    {
        private readonly Dictionary<string, MatchDTO> _matches;
        private readonly Dictionary<string, ActiveMatch> _activeMatches; 
        private readonly Dictionary<string, IMatchMakingServiceCallback> _usersInMatchMaking;
        private readonly Dictionary<string, IMatchServiceCallback> _usersInActiveMatch; 

        public MatchController(
            Dictionary<string, IMatchMakingServiceCallback> usersInMatchMaking,
            Dictionary<string, IMatchServiceCallback> usersInActiveMatch,
            Dictionary<string, MatchDTO> matches,
            Dictionary<string, ActiveMatch> activeMatches)
        {
            _matches = matches;
            _activeMatches = activeMatches;
            _usersInMatchMaking = usersInMatchMaking;
            _usersInActiveMatch = usersInActiveMatch;
        }

        public MatchDTO JoinToActiveMatch(string username, IMatchServiceCallback callback)
        {
            var match = _matches.Values.FirstOrDefault(m => m.Players.Values.Any(p => p.Username == username));

            if (!_activeMatches.ContainsKey(match.MatchCode)) 
            { 
                InitializeActiveMatch(match); 
            }

            var activeMatch = _activeMatches[match.MatchCode];
            var playerColor = match.Players.FirstOrDefault(p => p.Value.Username == username).Key;
            var playerState = new PlayerState { Username = username, Color = playerColor }; 

            activeMatch.Players.Add(playerState.Color, playerState);
            _usersInActiveMatch.Add(username, callback);

            if (activeMatch.Players.Count == match.NumberOfPlayers)
            {
                FinalizeMatchSetUp(match); 
            }

            return match; 
        }

        public BlockDTO GetCurrentBlock(string matchCode)
        {
            var activeMatch = _activeMatches[matchCode];
            return activeMatch.GetCurrentBlockAsDTO();
        }

        public GameResult PlaceBlock(string matchCode, int points)
        {
            var activeMatch = _activeMatches[matchCode];
            var matchResult = GameResult.None;

            activeMatch.Skips = 0; 

            PlayerState currentPlayer = activeMatch.GetCurrentPlayer();
            PlayerState nextPlayer = activeMatch.GetNextPlayer(); 

            currentPlayer.Puntuation += points;
            currentPlayer.RemoveBlock(activeMatch.Block);

            if (currentPlayer.BlocksList.Count == 0)
            {
                GameFinished(activeMatch, currentPlayer);
                matchResult = GameResult.Winner;
            }
            else
            {
                activeMatch.AdvanceTurn();

                foreach (var player in activeMatch.Players.Values)
                {
                    if (player.Color != currentPlayer.Color)
                    {
                        _usersInActiveMatch[player.Username].OnBlockPlaced();
                    }
                }

                SetCurrentBlock(activeMatch, currentPlayer, nextPlayer);
            }

            return matchResult; 
        }

        public void MakeMovement(string matchCode, Movement movement)
        {
            var activeMatch = _activeMatches[matchCode];

            foreach (var player in activeMatch.Players.Values)
            {
                if (player.Color != activeMatch.TurnOrder[activeMatch.Turn])
                {
                    try
                    {
                        _usersInActiveMatch[player.Username].OnOpponentMovement(movement);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(player.Username + " Fue el que trono");
                    }
                    
                }
            }
        }

        public GameResult SkipTurn(string matchCode)
        {
            var activeMatch = _activeMatches[matchCode];
            activeMatch.Skips++;

            PlayerState currentPlayer = activeMatch.GetCurrentPlayer();
            PlayerState nextPlayer = activeMatch.GetNextPlayer();

            var matchResult = GameResult.None; 

            if (activeMatch.Skips == activeMatch.Players.Count)
            {
                PlayerState winnerPlayer = activeMatch.Players.Values
                    .OrderByDescending(player => player.Puntuation).FirstOrDefault();

                matchResult = (currentPlayer.Username == winnerPlayer.Username) ? GameResult.Winner : GameResult.Losser;
                GameFinished(activeMatch, winnerPlayer);
            }
            else
            {
                activeMatch.AdvanceTurn();
                SetCurrentBlock(activeMatch, currentPlayer, nextPlayer); 
            }

            return matchResult;
        }

        public void LeaveActiveMatch(string username)
        {
            var matchCode = _activeMatches.FirstOrDefault(a => a.Value.Players.Values.Any(p => p.Username == username)).Key;
            var activeMatch = _activeMatches[matchCode];
            var playerColor = activeMatch.Players.FirstOrDefault(p => p.Value.Username == username).Key;
            var currentPlayer = activeMatch.GetCurrentPlayer();

            if (currentPlayer.Username == username)
            {
                activeMatch.Skips--;
                SkipTurn(matchCode);
            }

            if (activeMatch.Turn != 0)
            {
                activeMatch.Turn--;
            }

            activeMatch.Players.Remove(playerColor);
            activeMatch.TurnOrder.Remove(playerColor);
            _usersInActiveMatch.Remove(username);

            foreach (var player in activeMatch.Players.Values)
            {
                _usersInActiveMatch[player.Username].OnPlayerLeave(username, playerColor);
            }

            if (activeMatch.Players.Count == 0)
            {
                _activeMatches.Remove(matchCode);
            }
        }

        //Helpers Methods
        private void SetCurrentBlock(ActiveMatch activeMatch, PlayerState currentPlayer, PlayerState nextPlayer)
        {
            var blockDTO = new BlockDTO
            {
                Block = nextPlayer.GetRandomBlock(),
                Color = nextPlayer.Color
            };

            activeMatch.Block = blockDTO.Block;

            foreach (var player in activeMatch.Players.Values)
            {
                if (player.Color != currentPlayer.Color)
                {
                    _usersInActiveMatch[player.Username].OnCurrentBlockChanged(blockDTO);
                }
            }
        }

        private void InitializeActiveMatch(MatchDTO match)
        {
            _activeMatches[match.MatchCode] = new ActiveMatch
            {
                Block = Block.Block02,
                TurnOrder = RandomGenerator.ShuffleColors(match.Players.Keys.ToList())
            }; 
        }

        private void FinalizeMatchSetUp(MatchDTO match)
        {
            _matches.Remove(match.MatchCode);
            foreach (var player in match.Players.Values)
            {
                _usersInMatchMaking.Remove(player.Username);
            }
        }

        private void GameFinished(ActiveMatch activeMatch, PlayerState winnerPlayer)
        {
            var currentPlayer = activeMatch.GetCurrentPlayer();

            foreach (var player in activeMatch.Players.Values)
            {
                if (player.Username != currentPlayer.Username)
                {
                    var result = (player.Username == winnerPlayer.Username) ? GameResult.Winner : GameResult.Losser;
                    _usersInActiveMatch[player.Username].OnGameFinished(result);
                }
            }
        }
    }
}
