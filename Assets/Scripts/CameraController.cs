using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{

    public float sensitivity = 1f;
    
    private float _distance = 5f;
    private bool _isRotating = false;
    private Vector3 _mousePositionStart;
    private Quaternion _rotationStart;
    private Vector3 _currentMouse;
    private readonly Vector3 _lookAt = new Vector3(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        _distance = Vector3.Distance(_lookAt, transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
                _isRotating = true;
                _mousePositionStart = Input.mousePosition;
                _rotationStart = transform.rotation;
        }
        else if (Input.GetMouseButton(1))
        {
            _currentMouse = (_mousePositionStart - Input.mousePosition) * sensitivity;
//            _currentMouse.y *= -1f;
 
            var startingEuler = _rotationStart.eulerAngles;
 
            Quaternion rotation = Quaternion.Euler(ClampUpDownRotation(startingEuler.x + _currentMouse.y * .1f), startingEuler.y + _currentMouse.x * -.05f, 0);
 
            transform.rotation = rotation;
            transform.position = CalculatePosition();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _isRotating = false;
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
        {
            _distance = ClampDistance(_distance + 1);
            transform.position = CalculatePosition();
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
        {
            _distance = ClampDistance(_distance - 1);
            transform.position = CalculatePosition();
        }
    }

    Vector3 CalculatePosition()
    {
        return _lookAt - transform.rotation * (Vector3.forward * _distance);
    }

    float ClampUpDownRotation(float x)
    {
        if (x > 90.0f)
        {
            return 90.0f;
        }

        if (x < 8.0f)
        {
            return 8.0f;
        }

        return x;
    }

    float ClampDistance(float distance)
    {
        if (distance > 11.0f)
        {
            return 11.0f;
        }

        if (distance < 1.75f)
        {
            return 1.75f;
        }

        return distance;
    }
}
