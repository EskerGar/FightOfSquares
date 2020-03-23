using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISquare : MonoBehaviour
{
    public void SetSquare()
    {
        int square = (int)((transform.localScale.x * transform.localScale.y) / 100);
        GetComponentInChildren<TextMesh>().text = square.ToString();
    }
}
