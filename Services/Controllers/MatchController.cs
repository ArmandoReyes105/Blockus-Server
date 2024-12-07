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

            var block = new BlockDTO
            {
                Block = activeMatch.Block,
                Color = activeMatch.TurnOrder[activeMatch.Turn]
            };

            return block;
        }

        public GameResult PlaceBlock(string matchCode, int points)
        {
            var activeMatch = _activeMatches[matchCode];
            var matchResult = GameResult.None;

            activeMatch.Skips = 0; 

            PlayerState currentPlayer = GetCurrentPlayer(activeMatch);
            PlayerState nextPlayer = GetNextPlayer(activeMatch); 

            currentPlayer.Puntuation += points;
            currentPlayer.BlocksList.Remove(activeMatch.Block);

            if (currentPlayer.BlocksList.Count == 0)
            {
                GameFinished(activeMatch, currentPlayer);
                matchResult = GameResult.Winner;
            }
            else
            {
                activeMatch.Turn = GetNextTurn(activeMatch);

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

            PlayerState currentPlayer = GetCurrentPlayer(activeMatch);
            PlayerState nextPlayer = GetNextPlayer(activeMatch);

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
                activeMatch.Turn = GetNextTurn(activeMatch);
                SetCurrentBlock(activeMatch, currentPlayer, nextPlayer); 
            }

            return matchResult;
        }

        public void LeaveActiveMatch(string username)
        {
            var matchCode = _activeMatches.FirstOrDefault(a => a.Value.Players.Values.Any(p => p.Username == username)).Key;
            var activeMatch = _activeMatches[matchCode];
            var playerColor = activeMatch.Players.FirstOrDefault(p => p.Value.Username == username).Key;
            var currentPlayer = GetCurrentPlayer(activeMatch);

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

        private PlayerState GetCurrentPlayer(ActiveMatch activeMatch)
        {
            Color currentColorTurn = activeMatch.TurnOrder[activeMatch.Turn];
            return activeMatch.Players[currentColorTurn];
        }

        private PlayerState GetNextPlayer(ActiveMatch activeMatch)
        {
            int nextTurn = GetNextTurn(activeMatch);
            Color nextColorTurn = activeMatch.TurnOrder[nextTurn];
            return activeMatch.Players[nextColorTurn];
        }

        private void GameFinished(ActiveMatch activeMatch, PlayerState winnerPlayer)
        {
            var currentPlayer = GetCurrentPlayer(activeMatch);

            foreach (var player in activeMatch.Players.Values)
            {
                if (player.Username != currentPlayer.Username)
                {
                    var result = (player.Username == winnerPlayer.Username) ? GameResult.Winner : GameResult.Losser;
                    _usersInActiveMatch[player.Username].OnGameFinished(result);
                }
            }
        }

        private int GetNextTurn(ActiveMatch activeMatch) => 
             (activeMatch.Turn + 1) % activeMatch.Players.Count;
    }
}
