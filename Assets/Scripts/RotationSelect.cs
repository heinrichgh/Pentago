using System;
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
    private GameObject _rotatingPiece;

    private Rotation _currentRotation = Rotation.None;
    private Rotation _previousRotation = Rotation.None;
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
                _mousePositionStart = Input.mousePosition;
                RotationStart();
            }
        }

        if (_selectionStarted && Input.GetMouseButtonUp(0))
        {
            RotationEnd();
        } else if (_selectionStarted)
        {
            RotationInProgress(Input.mousePosition);
        }
    }

    void RotationStart()
    {
        _selectionStarted = true;
        var startPosition = _rotatingPiece.transform.position;
        var shiftedUp = new Vector3(startPosition.x, 0.0f, startPosition.z);
        _rotatingPiece.transform.position = shiftedUp;
    }

    void RotationInProgress(Vector3 mousePosition)
    {
        _currentRotation = CalculateRotationDirection(mousePosition);

        if (_currentRotation == _previousRotation) 
            return;
        
        var degreesToRotate = 180.0f * (int)_currentRotation;
        if (_previousRotation == Rotation.None)
        {
            degreesToRotate = 90.0f * (int)_currentRotation;
        }

        // Undo previous rotation
        if (_currentRotation == Rotation.None)
        {
            degreesToRotate = 90.0f * (int) _previousRotation * -1;
        }
        
        var upUnitVector = new Vector3(0.0f, 1.0f, 0.0f);
        _rotatingPiece.transform.Rotate(upUnitVector, degreesToRotate);

        _previousRotation = _currentRotation;
    }

    void RotationEnd()
    {
        var shiftedUp = _rotatingPiece.transform.position;
        var originalPosition = new Vector3(shiftedUp.x, -0.5f, shiftedUp.z);
        _rotatingPiece.transform.position = originalPosition;
        _selectionStarted = false;

        if (_currentRotation != Rotation.None)
        {
            GameController.instance.RotateBoardSection(_currentRotation, _rotatingPiece);
        }

        _currentRotation = Rotation.None;
        _previousRotation = Rotation.None;
    }

    Rotation CalculateRotationDirection(Vector3 mousePositionEnd)
    {
        var xDiff = _mousePositionStart.x - mousePositionEnd.x;
        
        // Must move at least 50 pixels to trigger a rotation
        if (xDiff > 50)
        {
            return Rotation.AntiClockwise;
        } 
        
        if (xDiff < -50)
        {
            return Rotation.Clockwise;
        }

        return Rotation.None;
    }
    
   
}