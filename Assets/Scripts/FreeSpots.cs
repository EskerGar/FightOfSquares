using System.Collections.Generic;
using UnityEngine;

public class FreeSpots : MonoBehaviour
{
    private List<GameObject> freeSpotsList;
    private List<GameObject> deactivateFreeSpotsList;
    public GameObject freeSpot;
    private GameObject lastCube;
    private int lengthList;
    private void Start()
    {
        freeSpotsList = new List<GameObject>();
        deactivateFreeSpotsList = new List<GameObject>();
    }

    public void GenerateFreeSpots()
    {
        float dX, dY;
        lastCube = GameManager.Instance.CubeBehaviour.LastCube;
        List<GameObject> cubesList = GameManager.Instance.CubeBehaviour.CubesList;
        lengthList = cubesList.Count - 1;
        Vector3 DPosition;
        for(int i = 0; i < cubesList.Count; i++)
        {
            if (CheckOnFirstCube(cubesList, lastCube))
                break;
            if ((GameManager.Instance.FirstPlayerTurn && cubesList[i].CompareTag("secondPlayerCube")) ||
               (!GameManager.Instance.FirstPlayerTurn && cubesList[i].CompareTag("firstPlayerCube")))
                continue;
            if (cubesList[i] == lastCube)
                continue;
            if (cubesList[i].CompareTag("firstPlayerCube"))
            {
                dX = -cubesList[i].transform.localScale.x / 2 - lastCube.transform.localScale.x / 2;
                dY = -cubesList[i].transform.localScale.y / 2 + lastCube.transform.localScale.y / 2;
                DPosition = new Vector3(dX, dY) ;
            }
            else
            {
                dX = cubesList[i].transform.localScale.x / 2 + lastCube.transform.localScale.x / 2;
                dY = cubesList[i].transform.localScale.y / 2 - lastCube.transform.localScale.y / 2;
                DPosition = new Vector3(dX, dY) ;
            }
            CreateOverlap(DPosition, cubesList[i]);
        }
    }

    private void CreateOverlap(Vector3 DPosition, GameObject cube)
    {
        Vector3 scale = lastCube.transform.localScale;
        Vector3 overlapPosition = cube.transform.position + DPosition;
        Collider2D[] results = new Collider2D[lengthList];
        CheckOverlaps(Physics2D.OverlapBoxNonAlloc(overlapPosition, scale, 0, results, LayerMask.GetMask("CubeLayer")), overlapPosition, scale);
        if(cube.CompareTag("firstPlayerCube"))
            DPosition = new Vector3(DPosition.x + cube.transform.localScale.x, DPosition.y + cube.transform.localScale.y);
        else
            DPosition = new Vector3(DPosition.x - cube.transform.localScale.x, DPosition.y - cube.transform.localScale.y);
        overlapPosition = cube.transform.position + DPosition;
        CheckOverlaps(Physics2D.OverlapBoxNonAlloc(overlapPosition, scale, 0, results, LayerMask.GetMask("CubeLayer")), overlapPosition, scale);
    }
    private void CheckOverlaps(int overlapsCount, Vector3 overlapPos, Vector3 scale)
    {
        if ((((overlapPos.x + scale.x / 2) <= 120f) && ((overlapPos.x - scale.x / 2) >= -180f) && ((overlapPos.y + scale.y / 2) <= 145f) && 
            ((overlapPos.y - scale.y / 2) >= -35f)) && overlapsCount == 0)
            CreateFreeSpot(overlapPos);
    }

    private bool CheckOnFirstCube(List<GameObject> cubesList, GameObject lastCube)
    {
        if ((cubesList[0] == lastCube || cubesList[1] == lastCube) && lastCube.CompareTag("firstPlayerCube"))
        {
            CreateFreeSpot(new Vector3(120f - lastCube.transform.localScale.x / 2, -35f + lastCube.transform.localScale.y / 2, 115f));
            return true;
        }else if ((cubesList[0] == lastCube || cubesList[1] == lastCube) && lastCube.CompareTag("secondPlayerCube"))
        {
            CreateFreeSpot(new Vector3(-180f + lastCube.transform.localScale.x / 2, 145f - lastCube.transform.localScale.y / 2, 115f));
            return true;
        }
            return false;
    }

    private void CreateFreeSpot(Vector3 position)
    {
        GameObject lastCube = GameManager.Instance.CubeBehaviour.LastCube;
        if (deactivateFreeSpotsList.Count == 0)
        {
            GameObject go = Instantiate(freeSpot);
            go.transform.localScale = lastCube.transform.localScale;
            go.transform.position = position;
            freeSpotsList.Add(go);
        }
        else
        {
            deactivateFreeSpotsList[0].transform.localScale = lastCube.transform.localScale;
            deactivateFreeSpotsList[0].transform.position = position;
            deactivateFreeSpotsList[0].SetActive(true);
            freeSpotsList.Add(deactivateFreeSpotsList[0]);
            deactivateFreeSpotsList.RemoveAt(0);
        }
    }

    public void DeactiveFreeSpots()
    {
        for (int i = 0; i < freeSpotsList.Count; i++)
        {
            freeSpotsList[i].SetActive(false);
            deactivateFreeSpotsList.Add(freeSpotsList[i]);
        }
        freeSpotsList.Clear();
    }
}
