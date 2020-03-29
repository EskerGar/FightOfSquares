using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITurn : MonoBehaviour
{
    public GameObject turn;

    private void Start()
    {
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        if (GameManager.Instance.FirstPlayerTurn)
            turn.GetComponent<Text>().text = "First Player Turn";
        else
            turn.GetComponent<Text>().text = "Second Player Turn";
    }
}
