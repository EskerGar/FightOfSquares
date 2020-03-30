using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeBehaviour : MonoBehaviour
{
    public GameObject cube;
    public List<GameObject> CubesList { get; private set; }
    public GameObject LastCube { get; private set; }
    public bool CubeCreated { get; private set; } = false;
    private UnityAction OnGenerateCube;

    private void Start()
    {
        CubesList = new List<GameObject>();
        OnGenerateCube += GameManager.Instance.FreeSpots.GenerateFreeSpots;
    }

    public void CubeRefresh() => CubeCreated = false;

    public void CreateCube(Vector3 scale)
    {
        if (!CubeCreated)
        {
            GameObject go = Instantiate(cube);
            go.transform.localScale = scale;
            go.transform.position = new Vector3(170f, 70f, 115);
            if (GameManager.Instance.FirstPlayerTurn)
            {
                go.tag = "firstPlayerCube";
                go.GetComponent<MeshRenderer>().material = Resources.Load("FirstPlayer") as Material;
            }
            else
            {
                go.tag = "secondPlayerCube";
                go.GetComponent<MeshRenderer>().material = Resources.Load("SecondPlayer") as Material;
            }
            CubesList.Add(go);
            LastCube = go;
            CubeCreated = true;
            LastCube.GetComponent<UISquare>().SetSquare();
            OnGenerateCube.Invoke();
        }
    }
}
