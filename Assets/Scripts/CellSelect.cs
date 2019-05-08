using System.Collections;
using System.Collections.Generic;
using Pentago;
using UnityEngine;
using UnityEngine.Serialization;

public class CellSelect : MonoBehaviour
{
    private Camera _mainCamera;
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
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 point = hit.point;
                Vector2Int gridPoint = Geometry.GridFromPoint(point);
                Debug.Log($"x: {gridPoint.x}, y: {gridPoint.y}, Enabled: {enabled}");
                GameController.instance.PlacePiece(gridPoint, hit.transform.gameObject);
            }
        }
    }

//    void PlacePiece(Vector2Int gridPoint)
//    {
//        var piecePrefab = GameController.instance.CurrentPlayer.PiecePrefab;
//        var position = Geometry.PointFromGrid(gridPoint);
//        var rotation = Quaternion.identity;
//
//        Instantiate(piecePrefab, position, rotation);
//        GameController.instance.NextPlayer();
//    }
    
   
}
