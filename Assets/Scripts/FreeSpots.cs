using cakeslice;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class FreeSpots : MonoBehaviour
{
    private List<GameObject> freeSpotsList;
    private List<GameObject> deactivateFreeSpotsList;
    public GameObject freeSpot;
    private GameObject lastCube;
    private int lengthList;
    private int freeSpotsCount = 0;
    private Vector2 borderY, borderX;
    private Vector3 overlapPosition, scale;
    public bool NoSpots { get; private set; }
    private void Awake()
    {
        freeSpotsList = new List<GameObject>();
        deactivateFreeSpotsList = new List<GameObject>();
    }

    public void StartGenerateFreeSpots()
    {
        GenerateFreeSpots();
        if (NoSpots)
            GameManager.Instance.ChangeTurnEvent();
    }

    private void GenerateFreeSpots()
    {
        lastCube = GameManager.Instance.CubeBehaviour.LastCube;
        var cubesList = GameManager.Instance.CubeBehaviour.CubesList;
        lengthList = cubesList.Count - 1;
        Vector3 currentCubeScale, currentCubePos;
        var firstCube = false;
        scale = lastCube.transform.localScale;
        for (int i = 0; i < cubesList.Count; i++)
        {
            if (CheckOnFirstCube(cubesList, lastCube))
            {
                firstCube = true;
                break;
            }
            if ((GameManager.Instance.FirstPlayerTurn && cubesList[i].CompareTag("secondPlayerCube")) ||
               (!GameManager.Instance.FirstPlayerTurn && cubesList[i].CompareTag("firstPlayerCube")))
                continue;
            if (cubesList[i] == lastCube)
                continue;
            currentCubeScale = cubesList[i].transform.localScale;
            currentCubePos = cubesList[i].transform.position;
            borderX = new Vector2(currentCubePos.x - currentCubeScale.x / 2, currentCubePos.x + currentCubeScale.x / 2);
            borderY = new Vector2(currentCubePos.y - currentCubeScale.y / 2, currentCubePos.y + currentCubeScale.y / 2);
            CreateOverlapPos(cubesList[i]);
        }
        if (freeSpotsCount == 0 && !firstCube)
        {
            GameManager.Instance.CubeBehaviour.CubesList.Remove(lastCube);
            Destroy(lastCube);
            NoSpots = true;
        }
        else
            NoSpots = false;
        if (GameManager.Instance.AiSide && GameManager.Instance.ReturnAI && freeSpotsCount > 0)
        {
            var aiTurn = Random.Range(1, freeSpotsCount);
            var cube = freeSpotsList[aiTurn - 1];
            lastCube.transform.position = cube.transform.position;
            GameManager.Instance.AddScoreEvent();
            GameManager.Instance.ChangeTurnEvent();
        }
        freeSpotsCount = 0;
    }

    private void CreateOverlapPos(GameObject cube)
    {
        float dX, dY;
        Vector3 DPosition, currentCubeScale = cube.transform.localScale;
        //CheckLeft
        dX = -currentCubeScale.x / 2 - scale.x / 2;
        dY = -currentCubeScale.y / 2 + scale.y / 2;
        DPosition = new Vector3(dX, dY);
        overlapPosition = cube.transform.position + DPosition;
        CheckY(1);
        DPosition = new Vector3(dX, -dY);
        overlapPosition = cube.transform.position + DPosition;
        CheckY(-1);
        //CheckUp
        DPosition = new Vector3(dX + currentCubeScale.x, dY + currentCubeScale.y);
        overlapPosition = cube.transform.position + DPosition;
        CheckX(-1);
        DPosition = new Vector3(dX + scale.x, dY + currentCubeScale.y);
        overlapPosition = cube.transform.position + DPosition;
        CheckX(1);
        //CheckRight
        dX = currentCubeScale.x / 2 + scale.x / 2;
        dY = currentCubeScale.y / 2 - scale.y / 2;
        DPosition = new Vector3(dX, dY);
        overlapPosition = cube.transform.position + DPosition;
        CheckY(-1);
        DPosition = new Vector3(dX, -dY);
        overlapPosition = cube.transform.position + DPosition;
        CheckY(1);
        //CheckDown
        DPosition = new Vector3(dX - currentCubeScale.x, dY - currentCubeScale.y);
        overlapPosition = cube.transform.position + DPosition;
        CheckX(1);
        DPosition = new Vector3(dX - scale.x, dY - currentCubeScale.y);
        overlapPosition = cube.transform.position + DPosition;
        CheckX(-1);
    }

    private void CheckX(int sign)
    {
        float cubeBorderMin, cubeBorderMax;
        do
        {
            CheckOverlaps(Physics2D.OverlapBoxNonAlloc(overlapPosition, scale, 0, new Collider2D[lengthList], LayerMask.GetMask("CubeLayer")), overlapPosition, scale);
            overlapPosition += sign * new Vector3(scale.x, 0, 0);
            cubeBorderMax = overlapPosition.x + scale.x / 2;
            cubeBorderMin = overlapPosition.x - scale.x / 2;
        } while (cubeBorderMin > borderX.x && cubeBorderMax < borderX.y);
    }
    private void CheckY(int sign)
    {
        float cubeBorderMin, cubeBorderMax;
        do
        {
            CheckOverlaps(Physics2D.OverlapBoxNonAlloc(overlapPosition, scale, 0, new Collider2D[lengthList], LayerMask.GetMask("CubeLayer")), overlapPosition, scale);
            overlapPosition += sign * new Vector3(0, scale.y);
            cubeBorderMax = overlapPosition.y + scale.y / 2;
            cubeBorderMin = overlapPosition.y - scale.y / 2;
        } while (cubeBorderMin > borderY.x && cubeBorderMax < borderY.y);
    }

    private void CheckOverlaps(int overlapsCount, Vector3 overlapPos, Vector3 scale)
    {
        if ((((overlapPos.x + scale.x / 2) <= 120f) && ((overlapPos.x - scale.x / 2) >= -180f) && ((overlapPos.y + scale.y / 2) <= 145f) &&
            ((overlapPos.y - scale.y / 2) >= -35f)) && overlapsCount == 0)
        {
            CreateFreeSpot(overlapPos); 
        }
    }

    private bool CheckOnFirstCube(List<GameObject> cubesList, GameObject lastCube)
    {
        if ((cubesList[0] == lastCube || cubesList[1] == lastCube) && lastCube.CompareTag("firstPlayerCube"))
        {
            CreateFreeSpot(new Vector3(120f - lastCube.transform.localScale.x / 2, -35f + lastCube.transform.localScale.y / 2, 115f));
            return true;
        }

        if ((cubesList[0] != lastCube && cubesList[1] != lastCube) ||
            !lastCube.CompareTag("secondPlayerCube")) return false;
        CreateFreeSpot(new Vector3(-180f + lastCube.transform.localScale.x / 2, 145f - lastCube.transform.localScale.y / 2, 115f));
        return true;
    }

    private void CreateFreeSpot(Vector3 position)
    {
        GameObject lastCube = GameManager.Instance.CubeBehaviour.LastCube;
        bool exist = false;
        foreach(var cube in freeSpotsList)
        {
            if (!cube.transform.position.Equals(position)) continue;
            exist = true;
            break;
        }
        if (deactivateFreeSpotsList.Count == 0 && !exist)
        {
            GameObject go = Instantiate(freeSpot);
            go.transform.localScale = lastCube.transform.localScale;
            go.transform.position = position;
            go.GetComponent<Outline>().enabled = false;
            freeSpotsList.Add(go);
            freeSpotsCount++;
        }
        else if(!exist)
        {
            deactivateFreeSpotsList[0].transform.localScale = lastCube.transform.localScale;
            deactivateFreeSpotsList[0].transform.position = position;
            deactivateFreeSpotsList[0].SetActive(true);
            deactivateFreeSpotsList[0].GetComponent<Outline>().enabled = false;
            freeSpotsList.Add(deactivateFreeSpotsList[0]);
            deactivateFreeSpotsList.RemoveAt(0);
            freeSpotsCount++;
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
