using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Players;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager: MonoBehaviour
{
   [SerializeField] private List<PlayerSettings> settingsList = new List<PlayerSettings>();
   [SerializeField] private Field field;
   
   private readonly List<Player> _playerList = new List<Player>();

   private void Start()
   {
       CreatePlayers();
       StartCoroutine(ChangeTurn());
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
       _playerList.Add(new HumanPlayer(!isFirstPlayer, settingsList[1]));

   }

   private IEnumerator ChangeTurn()
   {
       while (true)
       {
           var player = GetTurnPlayer();
           player.DoTurn();
           yield return new WaitWhile(() => player.IsYourTurn);
           ChangePlayers(player);
       }
   }

   private Player GetTurnPlayer()
   {
       return _playerList.First(player => player.IsYourTurn);
   }

   private void ChangePlayers(Player prevPlayer)
   {
       foreach (var player in _playerList.Where(player => !player.Equals(prevPlayer)))
       {
           player.IsYourTurn = true;
       }
   }
}
