using System.Collections;
using System.Collections.Generic;
using Pentago;
using UnityEngine;
using UnityEngine.Serialization;

public class CellSelect : MonoBehaviour
{
    private Camera _mainCamera;
    private GameObject _previewPiece;
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
                PlacePiece(hit);
            }
            else
            {
                PlacePreviewPiece(hit);
            }
        }
    }

    private void PlacePiece(RaycastHit hit)
    {
        Vector3 point = hit.point;
        Vector2Int gridPoint = Geometry.GridFromPoint(point);
        GameController.instance.PlacePiece(gridPoint, hit.transform.gameObject);
        Destroy(_previewPiece);
    }

    private void PlacePreviewPiece(RaycastHit hit)
    {
        Destroy(_previewPiece);
        Vector3 point = hit.point;
        Vector2Int gridPoint = Geometry.GridFromPoint(point);
        
        if (!GameController.instance.Grid.IsPlaceEmpty(gridPoint.y, gridPoint.x))
            return;
        
        var position = Geometry.PointFromGrid(gridPoint);
        var rotation = Quaternion.identity;
        var piecePrefab = GameController.instance.CurrentPlayer.PiecePrefab;
        _previewPiece = Instantiate(piecePrefab, position, rotation);
    }
}
