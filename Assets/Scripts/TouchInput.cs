using System;
using Cubes;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [SerializeField] private float zoomOutMin = 1f;
    
    private float _zoomOutMax;
    private GameObject _prevFreeSpot;
    private Camera _camera;
    private const float CameraMoveSpeed = 2f;
    private Vector3 _originalMin;
    private Vector3 _originalMax;

    private void Awake()
    {
        _camera = Camera.main;
        _originalMin  = _camera.ViewportToWorldPoint(new Vector2(0, 0));
        _originalMax = _camera.ViewportToWorldPoint(new Vector2(1, 1));
        _zoomOutMax = _camera.orthographicSize;
    }

    private void Update()
    {
        switch (Input.touchCount)
        {
            case 1 when Input.GetTouch(0).phase == TouchPhase.Began:
                TouchOnFreeSpot();
                break;
            case 2 when (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved):
                ZoomTouch();
                break;
            case 1 when Input.GetTouch(0).phase == TouchPhase.Moved:
                MovingCamera();
                break;
        }
    }

    private void TouchOnFreeSpot()
    {
        var ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);
        var hit = Physics2D.Raycast(ray.origin, ray.direction);
        var freeSpot = hit.transform.gameObject.GetComponent<FreeSpotBehaviour>();
        if (freeSpot == null) return;
        if(_prevFreeSpot != null && _prevFreeSpot.Equals(freeSpot.gameObject))
        {
            freeSpot.OnTouch();
            _prevFreeSpot = null;
        }
        else 
        {
            if(_prevFreeSpot != null)
                _prevFreeSpot.GetComponent<FreeSpotBehaviour>().DeActivateOutline();
            _prevFreeSpot = freeSpot.gameObject;
            freeSpot.ActivateOutline(); 
        } 
    }

    private void ZoomTouch()
    {
        var touchZero = Input.GetTouch(0);
        var touchOne = Input.GetTouch(1);
            
        var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            
        var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            
        var deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            
        Zoom(-deltaMagnitudeDiff * 0.01f); 
    }

    private void Zoom(float increment)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - increment, zoomOutMin, _zoomOutMax);
    }

    private void MovingCamera()
    {
        var touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        _camera.transform.Translate(
            -touchDeltaPosition.x * CameraMoveSpeed * Time.deltaTime, 
            -touchDeltaPosition.y * CameraMoveSpeed * Time.deltaTime, 
            0);
        CheckBorders();
    }

    private void CheckBorders()
    {
        var min = _camera.ViewportToWorldPoint(new Vector2(0, 0));
        var max = _camera.ViewportToWorldPoint(new Vector2(1, 1));
        var deltaVectorMin = min - _originalMin;
        var deltaVectorMax = max - _originalMax;
        CheckSide(() => min.x < _originalMin.x, new Vector3(1f, 0f) * Mathf.Abs(deltaVectorMin.x));
        CheckSide(() => max.x > _originalMax.x, new Vector3(-1f, 0f) * Mathf.Abs(deltaVectorMax.x));
        CheckSide(() => min.y < _originalMin.y, new Vector3(0f, 1f) * Mathf.Abs(deltaVectorMin.y));
        CheckSide(() => max.y > _originalMax.y, new Vector3(0f, -1f) * Mathf.Abs(deltaVectorMax.y));
        
    }
    
    private void CheckSide(Func<bool> condition, Vector3 addVector)
    {
        if (condition())
            _camera.transform.position += addVector;
    }
}