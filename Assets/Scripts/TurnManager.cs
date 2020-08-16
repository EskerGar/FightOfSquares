using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cubes;
using Players;
using UnityEngine;
using static Players.PlayerStatistic;
using Random = UnityEngine.Random;

public class TurnManager: MonoBehaviour
{
   [SerializeField] private List<PlayerSettings> settingsList = new List<PlayerSettings>();
   [SerializeField] private Field field;

   public PlayerListHandler GetHandler => _playerListHandler;
   public event Action OnGameEnd;

   private PlayerListHandler _playerListHandler;
   private readonly List<Player> _playerList = new List<Player>();
   private float _allSquare;
   
   private void Awake()
   {
       CreatePlayers();
       _allSquare = field.GetSquare();
   }

   private void Start()
   {
       StartCoroutine(StartGame());
   }

   private void CreatePlayers()
   {
       bool isFirstPlayer;
       isFirstPlayer = Random.Range(0, 2) > 0;
       for (int i = 0; i < settingsList.Count; i++)
       {
           field.InstallSettings(settingsList[i], i);
       }
       
       _playerList.Add(new HumanPlayer(isFirstPlayer,  settingsList[0]));
       _playerList.Add(new AiPlayer(!isFirstPlayer, settingsList[1]));
       _playerListHandler = new PlayerListHandler(_playerList);
   }

   private IEnumerator StartGame()
   {
       yield return StartCoroutine(ChangeTurn());
       OnGameEnd?.Invoke();
       _playerList[0].OffSquareCubes();
       _playerList[1].OffSquareCubes();
       EndGame();
   }
   
   private IEnumerator ChangeTurn()
   {
       while (!CheckScore())
       {
           var player = GetTurnPlayer();
           player.DoTurn();
           yield return new WaitWhile(() => FsPool.FreeSpotsList.Count > 0);
           ChangePlayers(player);
       }
   }

   private void EndGame()
   {
       if(!_playerList.OfType<HumanPlayer>().Count().Equals(_playerList.Count))
           return;
       var winner = _playerListHandler.GetWinnerName();
       if(winner == null)
       {
           AddDraw();
       }
       else
       {
           
       }
   }

   private bool CheckScore()
   {
       var score = _playerList.Aggregate<Player, float>(0, (current, player) => current + player.Score);
       return Math.Abs(score - _allSquare) < 0.1f;
   }
   

   private Player GetTurnPlayer() => _playerList.First(player => player.IsYourTurn);

   private void ChangePlayers(Player prevPlayer)
   {
       foreach (var player in _playerList.Where(player => !player.Equals(prevPlayer)))
       {
           prevPlayer.ControlLastCube();
           prevPlayer.IsYourTurn = false;
           player.IsYourTurn = true;
       }
   }
}
