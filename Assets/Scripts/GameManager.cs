using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public bool FirstPlayerTurn { get; private set; }
    public FreeSpots FreeSpots { get; private set; }
    public CubeBehaviour CubeBehaviour { get; private set; }
    private void Awake() 
    { 
        if (Instance == null) 
            Instance = this; 
    }

    private void Start()
    {
        GetFirstTurnRandom();
        CubeBehaviour = GetComponent<CubeBehaviour>();
        FreeSpots = GetComponent<FreeSpots>();
    }

    public void GetFirstTurnRandom() => FirstPlayerTurn = Random.Range(0, 2) > 0 ? true : false;

    public void NextTurn()
    {
        CubeBehaviour.CubeRefresh();
        FirstPlayerTurn = !FirstPlayerTurn;
    }
}
