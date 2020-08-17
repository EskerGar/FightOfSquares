using System;
using System.Collections.Generic;
using Cubes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using PlayerSettings = Players.PlayerSettings;

public class Field : MonoBehaviour
{
    [SerializeField] private List<GameObject> cornersList;
    [SerializeField] private List<GameObject> bordersList;
    
    private float _leftBorder;
    private float _rightBorder;
    private float _upBorder;
    private float _downBorder;

    public float GetSquare()
    {
        var square =  FindSquare(cornersList[0].transform.position, cornersList[1].transform.position);
        Assert.IsTrue(square % 2 == 0, "square % 2 == 0, square = " + square);
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

    private void Start()
    {
        Assert.IsTrue(Mathf.Abs(cornersList[0].transform.position.x) > Mathf.Abs(cornersList[1].transform.position.x));
        _leftBorder = cornersList[0].transform.position.x;
        _upBorder = cornersList[0].transform.position.y;
        _rightBorder = cornersList[1].transform.position.x;
        _downBorder = cornersList[1].transform.position.y;
        DrawField();
    }

    private void DrawField()
    {
        var center = FindCenter(cornersList[0].transform.position, cornersList[1].transform.position);
        var horizontalSize = FindSize(cornersList[0].transform.position,
                                            new Vector3(cornersList[1].transform.position.x, cornersList[0].transform.position.y));
        var verticalSize = FindSize(cornersList[1].transform.position,
                                          new Vector3(cornersList[1].transform.position.x, cornersList[0].transform.position.y));
        bordersList[0].SetParametrs(
            new Vector3(center.x, cornersList[0].transform.position.y + 0.1f), 
            new Vector3(0.2f, horizontalSize + 0.4f));
        bordersList[1].SetParametrs(
            new Vector3(center.x, cornersList[1].transform.position.y - 0.1f),
            new Vector3(0.2f, horizontalSize + 0.4f));
        bordersList[2].SetParametrs(
            new Vector3(cornersList[0].transform.position.x - 0.1f, center.y ),
            new Vector3(verticalSize, 0.2f));
        bordersList[3].SetParametrs(
            new Vector3(cornersList[1].transform.position.x + 0.1f, center.y ),
            new Vector3(verticalSize, 0.2f));
    }
    
    
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.red;
        foreach (var corner in cornersList)
        {
            Gizmos.DrawCube(corner.transform.position, corner.transform.localScale);
        }
    
        var square = FindSquare(cornersList[0].transform.position, cornersList[1].transform.position);
        var center = FindCenter(cornersList[0].transform.position, cornersList[1].transform.position);
        Handles.Label(center, square.ToString());
#endif
    }

    private int FindSquare(Vector3 a, Vector3 b) => (int)(Mathf.Abs(a.x - b.x) * Mathf.Abs(a.y - b.y));

    private Vector3 FindCenter(Vector3 a, Vector3 b) => new Vector3((a.x + b.x )/ 2, (a.y + b.y )/ 2 );
    
    private float FindSize(Vector3 a, Vector3 b) => new Vector2(b.x - a.x, b.y - a.y).magnitude;
}

