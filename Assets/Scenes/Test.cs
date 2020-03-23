using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var overlap in Physics2D.OverlapBoxAll(new Vector3(16.9f, -13.92f, 115f), gameObject.transform.localScale, 0))
            Debug.Log(overlap);
    }

}
