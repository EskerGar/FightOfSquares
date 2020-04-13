using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    private void OnMouseEnter()
    {
        gameObject.GetComponent<Outline>().enabled = true;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Outline>().enabled = false;
    }
}
