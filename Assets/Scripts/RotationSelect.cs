using System;
using System.Collections;
using System.Collections.Generic;
using Pentago;
using UnityEngine;
using UnityEngine.Serialization;

public class RotationSelect : MonoBehaviour
{
    public float upDistance;
    public float sensitivity = 1.0f;

    private Camera _mainCamera;
    private bool _selectionStarted;
    private Vector3 _mousePositionStart;
    private GameObject _rotatingPiece;
    private Rotation _currentRotation = Rotation.None;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Quaternion _rotation;

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
                RotationStart(hit);
            }
        }

        if (_selectionStarted && Input.GetMouseButtonUp(0))
        {
            RotationEnd();
        }
        else if (_selectionStarted)
        {
            RotationInProgress();
        }
    }

    private Vector3 _hitSpot;
    void RotationStart(RaycastHit hit)
    {
        _hitSpot = hit.point;
        _selectionStarted = true;
        _rotatingPiece = hit.transform.gameObject;
        _mousePositionStart = Input.mousePosition;
        _startPosition = _rotatingPiece.transform.position;
        _startRotation = _rotatingPiece.transform.rotation;
        _rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        var shiftedUp = new Vector3(_startPosition.x, _startPosition.y + upDistance, _startPosition.z);
        _rotatingPiece.transform.position = shiftedUp;
    }

    void RotationInProgress()
    {
        var angleVector = (_mousePositionStart - Input.mousePosition) * sensitivity;
        
        // Calculate the appropriate direction of rotation based on where in the board section you started dragging.
        if (_hitSpot.x < _rotatingPiece.transform.position.x)
        {
            angleVector.y *= -1.0f;
        }

        if (_hitSpot.z > _rotatingPiece.transform.position.z)
        {
            angleVector.x *= -1.0f;
        }

        var angle = 0.5f * angleVector.x + 0.5f * angleVector.y;
        
        RotateObject(angle);
        
        if (angle > 45)
        {
            _currentRotation = Rotation.Clockwise;
        }
        else if (angle < -45)
        {
            _currentRotation = Rotation.AntiClockwise;
        }
        else
        {
            _currentRotation = Rotation.None;
        }
    }

    void RotateObject(float angle)
    {
        var startingEuler = _startRotation.eulerAngles;
        Quaternion rotation = Quaternion.Euler(startingEuler.x, startingEuler.y + ClampRotation(angle), startingEuler.z);

        _rotatingPiece.transform.rotation = rotation;
    }

    float ClampRotation(float rotate)
    {
        if (rotate > 90.0f)
        {
            return 90.0f;
        }
        
        if (rotate < -90.0f)
        {
            return -90.0f;
        }

        return rotate;
    }

    void RotationEnd()
    {
        _rotatingPiece.transform.position = _startPosition;
        _selectionStarted = false;

        if (_currentRotation != Rotation.None)
        {
            if (_currentRotation == Rotation.Clockwise)
            {
                RotateObject(90.0f);
            } 
            else if (_currentRotation == Rotation.AntiClockwise)
            {
                RotateObject(-90.0f);
            }
            GameController.instance.RotateBoardSection(_currentRotation, _rotatingPiece);
        }
        else
        {
            RotateObject(0.0f);
        }

        _currentRotation = Rotation.None;
    }
}