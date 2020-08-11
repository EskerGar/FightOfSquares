using System;
using System.Collections.Generic;
using System.Linq;
using Players;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiStatistics: MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private List<Text> statsUiList;
    [SerializeField] private List<Text> scoresUiList;

    private PlayerListHandler _listHandler;
    private void Start()
    {
        _listHandler = turnManager.GetHandler;
        _listHandler.SubscribeOnScoreEvent(scoresUiList);
        SetPlayerStats();
    }

    private void SetPlayerStats()
    {
        var statsList = _listHandler.GetStatistic();
        if(statsList.Count == 0) return;
        for (var i = 0; i < statsList.Count; i++)
        {
            var stats = statsList[i];
            statsUiList[i].text = stats.NickName + "\n" + stats.WinsAmount + " / " + stats.LosesAmount + " / " + stats.DrawsAmount;
        }
    }
}