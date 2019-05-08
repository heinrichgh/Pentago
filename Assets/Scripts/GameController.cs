using System.Collections;
using System.Collections.Generic;
using Pentago;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;

//    public GameObject BoardSectionBottomLeft;
//    public GameObject BoardSectionBottomRight;
//    public GameObject BoardSectionTopRight;
//    public GameObject BoardSectionTopLeft;

    public GridState Grid;

    public Player CurrentPlayer;
    public Player OtherPlayer;
    // Start is called before the first frame update

    private CellSelect _cellSelector;
    private RotationSelect _rotationSelector;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        Grid = new GridState();
        CurrentPlayer = new Player(playerOnePrefab, 1);
        OtherPlayer = new Player(playerTwoPrefab, 2);

        _cellSelector = GetComponent<CellSelect>();
        _rotationSelector = GetComponent<RotationSelect>();
        _rotationSelector.enabled = false;
    }

    void NextPlayer()
    {
        var temp = CurrentPlayer;
        CurrentPlayer = OtherPlayer;
        OtherPlayer = temp;
        _cellSelector.enabled = true;
        _rotationSelector.enabled = false;
    }
    
    public void PlacePiece(Vector2Int gridPoint, GameObject boardSection)
    {
        var playerNumber = CurrentPlayer.PlayerNumber;

        if (!Grid.PlacePiece(playerNumber, gridPoint.y, gridPoint.x))
        {
            return;
        }

        var position = Geometry.PointFromGrid(gridPoint);
        var rotation = Quaternion.identity;
        var piecePrefab = CurrentPlayer.PiecePrefab;
        var newPiece = Instantiate(piecePrefab, position, rotation);
        newPiece.transform.parent = boardSection.transform;
        NextRotation();
    }

    public void RotateBoardSection(Rotation rotation)
    {
        Debug.Log("ROTATE: " + rotation);
        NextPlayer();
    }

    void NextRotation()
    {
        _cellSelector.enabled = false;
        _rotationSelector.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
