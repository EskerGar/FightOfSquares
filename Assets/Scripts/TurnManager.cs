using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cubes;
using Players;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager: MonoBehaviour
{
   [SerializeField] private List<PlayerSettings> settingsList = new List<PlayerSettings>();
   [SerializeField] private Field field;
   
   private readonly List<Player> _playerList = new List<Player>();
   private float _allSquare;

   private void Start()
   {
       CreatePlayers();
       StartCoroutine(StartGame());
       _allSquare = field.GetSquare();
   }

   private void CreatePlayers()
   {
       bool isFirstPlayer;
       isFirstPlayer = Random.Range(0, 2) > 0;
       for (int i = 0; i < settingsList.Count; i++)
       {
           field.InstallSettings(settingsList[i], i);
       }
       
       _playerList.Add(new AiPlayer(isFirstPlayer,  settingsList[0]));
       _playerList.Add(new AiPlayer(!isFirstPlayer, settingsList[1]));

   }

   private IEnumerator StartGame()
   {
       var turnCoroutine = StartCoroutine(ChangeTurn());
       yield return new WaitUntil(CheckScore);
       StopCoroutine(turnCoroutine);
       
   }
   
   private IEnumerator ChangeTurn()
   {
       while (true)
       {
           var player = GetTurnPlayer();
           player.DoTurn();
           yield return new WaitWhile(() => FsPool.FreeSpotsList.Count > 0);
           ChangePlayers(player);
       }
   }

   private bool CheckScore()
   {
       var score = _playerList.Aggregate<Player, float>(0, (current, player) => current + player.Score);
       return Math.Abs(score - _allSquare) < 0.1f;
   }
   

   private Player GetTurnPlayer()
   {
       return _playerList.First(player => player.IsYourTurn);
   }

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
