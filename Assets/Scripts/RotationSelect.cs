using System.Collections;
using System.Collections.Generic;
using Pentago;
using UnityEngine;
using UnityEngine.Serialization;

public class RotationSelect : MonoBehaviour
{
    private Camera _mainCamera;

    private bool _selectionStarted;

    private Vector3 _mousePositionStart;
    private Vector3 _mousePositionEnd;
    private GameObject _rotatingPiece;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0) && !_selectionStarted)
            {
                _rotatingPiece = hit.transform.gameObject;
                Debug.Log($"x: {Input.mousePosition.x}, y: {Input.mousePosition.y}");
                _mousePositionStart = Input.mousePosition;
                RotationStart();
            }
        }

        if (_selectionStarted && Input.GetMouseButtonUp(0))
        {
            Debug.Log($"x: {Input.mousePosition.x}, y: {Input.mousePosition.y}");
            _mousePositionEnd = Input.mousePosition;
            RotationEnd();
        }
    }

    void RotationStart()
    {
        _selectionStarted = true;
        var startPosition = _rotatingPiece.transform.position;
        var shiftedUp = new Vector3(startPosition.x, 0.0f, startPosition.z);
        _rotatingPiece.transform.position = shiftedUp;
    }

    void RotationEnd()
    {
        var shiftedUp = _rotatingPiece.transform.position;
        var originalPosition = new Vector3(shiftedUp.x, -0.5f, shiftedUp.z);
        _rotatingPiece.transform.position = originalPosition;
        _selectionStarted = false;
        
        var rotation = CalculateRotationDirection();
        GameController.instance.RotateBoardSection(rotation);
    }

    Rotation CalculateRotationDirection()
    {
        var xDiff = _mousePositionStart.x - _mousePositionEnd.x;
        return xDiff > 0 ? Rotation.AntiClockwise : Rotation.Clockwise;
    }
    
   
}