﻿using UnityEngine;

public class Generation: MonoBehaviour
{
    public void GenerationCube()
    {
        Vector3 scale = new Vector3(Random.Range(1,6) * 10, Random.Range(1, 6) * 10, 1);
        //Vector3 scale = new Vector3(6 * 10, 6 * 10, 1);
        GameManager.Instance.CubeBehaviour.CreateCube(scale);
    }

}
