using System.Collections.Generic;
using Players;
using UnityEngine.UI;

public class PlayerListHandler
{
    private readonly List<Player> _playerList;
    
    public PlayerListHandler(List<Player> players)
    {
        _playerList = players;
    }

    public List<PlayerStatistic> GetStatistic()
    {
        var statsList = new List<PlayerStatistic>();
        foreach (var player in _playerList)
        {
            if(player is HumanPlayer humanPlayer)
                statsList.Add(humanPlayer.PlayerStatistic);
        }

        return statsList;
    }

    public void SubscribeOnScoreEvent(List<Text> uiScoreList)
    {
        for (int i = 0; i < uiScoreList.Count; i++)
        {
            uiScoreList[i].GetComponent<UiScore>().SubscribeOnAddScore(_playerList[i]);
        }
    }
}