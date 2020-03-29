using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    public GameObject firstScore, secondScore;
    public void ChangeScore()
    {
        firstScore.GetComponent<Text>().text = "First Player: " + GameManager.Instance.FirstPlayerScore.ToString();
        secondScore.GetComponent<Text>().text = "Second Player: " + GameManager.Instance.SecondPlayerScore.ToString();
    }
}
