using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public bool FirstPlayerTurn { get; private set; }
    public FreeSpots FreeSpots { get; private set; }
    public CubeBehaviour CubeBehaviour { get; private set; }
    public GameObject UI;
    private UIScore uiScore;
    private EndGame endGame;
    private UITurn uiTurn;
    public float FirstPlayerScore { get; private set; } = 0;
    public float SecondPlayerScore { get; private set; } = 0;

    private UnityAction OnAddScore, OnEndGame, OnTurnChange;
    private void Awake() 
    { 
        if (Instance == null) 
            Instance = this;
        CubeBehaviour = GetComponent<CubeBehaviour>();
        FreeSpots = GetComponent<FreeSpots>();
        endGame = GetComponent<EndGame>();
    }

    private void Start()
    {
        GetFirstTurnRandom();
        uiScore = UI.GetComponent<UIScore>();
        uiTurn = UI.GetComponent<UITurn>();
        OnAddScore += AddScore;
        OnAddScore += uiScore.ChangeScore;
        OnEndGame += endGame.GameEnded;
        OnTurnChange += NextTurn;
        OnTurnChange += uiTurn.ChangeTurn;
    }

    public void GetFirstTurnRandom() => FirstPlayerTurn = Random.Range(0, 2) > 0 ? true : false;

    public void NextTurn()
    {
        FreeSpots.DeactiveFreeSpots();
        CubeBehaviour.CubeRefresh();
        FirstPlayerTurn = !FirstPlayerTurn;
    }

    public void AddScoreEvent() => OnAddScore?.Invoke();
    public void EndGameEvent() => OnEndGame?.Invoke();
    public void ChangeTurnEvent() => OnTurnChange?.Invoke();
    private void AddScore()
    {
        GameObject cube = CubeBehaviour.LastCube;
        float score = (cube.transform.localScale.x * cube.transform.localScale.y) / 100;
        if (cube.CompareTag("firstPlayerCube"))
            FirstPlayerScore += score;
        else SecondPlayerScore += score;
    }
}
