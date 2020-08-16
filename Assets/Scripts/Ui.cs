using System;
using System.Collections.Generic;
using System.Linq;
using Players;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Ui: MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private GameObject endGameUi;
    [SerializeField] private List<Text> statsUiList;
    [SerializeField] private List<Text> scoresUiList;

    private PlayerListHandler _listHandler;
    private void Start()
    {
        endGameUi.SetActive(false);
        _listHandler = turnManager.GetHandler;
        _listHandler.SubscribeOnScoreEvent(scoresUiList);
        SetPlayerStats();
        turnManager.OnGameEnd += ShowEndGame;
    }

    private void SetPlayerStats()
    {
        var statsList = _listHandler.GetStatistic();
        if(statsList.Count == 0) return;
        for (var i = 0; i < statsList.Count; i++)
        {
            var stats = statsList[i];
            if(stats == null) continue;
            statsUiList[i].text = stats.NickName + "\n" + stats.WinsAmount + " / " + stats.LosesAmount + " / " + stats.DrawsAmount;
        }
    }

    private void ShowEndGame()
    {
        endGameUi.SetActive(true);
        var winner = _listHandler.GetWinnerName();
        endGameUi.GetComponentInChildren<Text>().text = winner == null ? "Draw" : winner + " Win!";
    }
}