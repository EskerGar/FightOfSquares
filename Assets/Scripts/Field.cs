using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class Field : MonoBehaviour
{
    [SerializeField]private List<GameObject> cornersList = new List<GameObject>();
    
    private float _leftBorder;
    private float _rightBorder;
    private float _upBorder;
    private float _downBorder;

    public float GetSquare()
    {
        var square =  FindSquare(cornersList[0].transform.position, cornersList[1].transform.position);
        Assert.IsTrue(square % 2 == 0);
        return square;
    }

    public bool CheckFieldBorders(Vector3 position, Vector3 size)
    {
        var halfSize = size / 2;
        return position.x + halfSize.x <= _rightBorder && position.x - halfSize.x >= _leftBorder && position.y + halfSize.y <= _upBorder &&
               position.y - halfSize.y >= _downBorder;
    }

    public void InstallSettings(PlayerSettings settings, int i)
    {
        if (i == 0)
            settings.SetSettings(cornersList[0].transform.position, 1, true);
        else
            settings.SetSettings(cornersList[1].transform.position, -1, false);

    }

    public Vector3 GetStartPos(int playerNumber) => cornersList[playerNumber].transform.position;

    private void Start()
    {
        Assert.IsTrue(Mathf.Abs(cornersList[0].transform.position.x) > Mathf.Abs(cornersList[1].transform.position.x));
        _leftBorder = cornersList[0].transform.position.x;
        _upBorder = cornersList[0].transform.position.y;
        _rightBorder = cornersList[1].transform.position.x;
        _downBorder = cornersList[1].transform.position.y;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var corner in cornersList)
        {
            Gizmos.DrawCube(corner.transform.position, corner.transform.localScale);
        }

        var square = FindSquare(cornersList[0].transform.position, cornersList[1].transform.position);
        var center = FindCenter(cornersList[0].transform.position, cornersList[1].transform.position);
        Handles.Label(center, square.ToString());
    }

    private float FindSquare(Vector3 a, Vector3 b) => Mathf.Abs(a.x - b.x) * Mathf.Abs(a.y - b.y);

    private Vector3 FindCenter(Vector3 a, Vector3 b) => new Vector3((a.x + b.x )/ 2, (a.y + b.y )/ 2 );
}

