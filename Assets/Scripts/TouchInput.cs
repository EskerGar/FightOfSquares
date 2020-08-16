using System;
using Cubes;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [SerializeField] private float zoomOutMin = 1f;
    
    private float _zoomOutMax;
    private GameObject _prevFreeSpot;
    private Camera _camera;
    private float _width;
    private float _height;

    private void Awake()
    {
        _camera = Camera.main;
        _width  = Screen.width / 2.0f;
        _height = Screen.height / 2.0f;
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
        //MovingCamera();
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
        _camera.transform.Translate(-touchDeltaPosition.x * 2f * Time.deltaTime, -touchDeltaPosition.y * 2f * Time.deltaTime, 0);
    }
    
}