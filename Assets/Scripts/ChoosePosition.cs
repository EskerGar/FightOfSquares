using UnityEngine;
using UnityEngine.Events;

public class ChoosePosition : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject lastCube = GameManager.Instance.CubeBehaviour.LastCube;
        lastCube.transform.position = transform.position;
        GameManager.Instance.AddScoreEvent();
        GameManager.Instance.ChangeTurnEvent();
    }
}
