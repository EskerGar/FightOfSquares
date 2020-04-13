using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public GameObject button, winner;

    public  void GameEnded()
    {
        button.SetActive(false);
        winner.SetActive(true);
        gameObject.GetComponent<CubeBehaviour>().enabled = false;
        gameObject.GetComponent<FreeSpots>().enabled = false;
        winner.GetComponent<Text>().text = WhoIsWin();
    }

    private string WhoIsWin()
    {
        if (GameManager.Instance.FirstPlayerScore > GameManager.Instance.SecondPlayerScore)
            return "First Player Win!";
        else if (GameManager.Instance.FirstPlayerScore < GameManager.Instance.SecondPlayerScore)
            return "Second Player Win!";
        else return "Draw";
    }
}
