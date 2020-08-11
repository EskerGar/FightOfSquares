using System;
using Players;
using UnityEngine;
using UnityEngine.UI;

public class UiScore: MonoBehaviour
{
    private Text _scoreText;

    private void Start()
    {
        _scoreText = GetComponent<Text>();
    }

    public void SubscribeOnAddScore(Player player)
    {
        player.OnAddScore += SetScore;
    }

    private void SetScore(int score)
    {
        _scoreText.text = "Score: " +  score;
    }

}