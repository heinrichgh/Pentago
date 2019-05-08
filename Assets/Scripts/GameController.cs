using System.Collections;
using System.Collections.Generic;
using Pentago;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public Text playerWin;
    public Text restartGame;
    public GridState Grid;

    public Player CurrentPlayer;
    public Player OtherPlayer;
    // Start is called before the first frame update

    private CellSelect _cellSelector;
    private RotationSelect _rotationSelector;
    private bool _canRestart;

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
        _cellSelector.enabled = true;
        _rotationSelector.enabled = false;
        _canRestart = false;
        playerWin.text = "";
        restartGame.text = "";
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

        if (!CheckWinner())
        {
//            NextPlayer();
            NextRotation();
        }
    }

    public void RotateBoardSection(Rotation rotation, GameObject rotatingSection)
    {
        var rotatingSectionPosition = rotatingSection.transform.position;
        var gridPoint = Geometry.GridFromPoint(new Vector3(rotatingSectionPosition.x, 0.0f, rotatingSectionPosition.z));

        Grid.Rotate(rotation, gridPoint.y, gridPoint.x);
        
        if (!CheckWinner())
        {
            NextPlayer();
        }
    }

    void NextRotation()
    {
        _cellSelector.enabled = false;
        _rotationSelector.enabled = true;
    }

    bool CheckWinner()
    {
        var winner = Grid.CheckWinConditions();
        if (winner != 0)
        {
            if (winner == -1)
            {
                playerWin.text = $" Stalemate! ";
            }
            else
            {
                playerWin.text = $"Player {winner} won!";
            }
            restartGame.text = "Press 'R' to play again";

            _cellSelector.enabled = false;
            _rotationSelector.enabled = false;
            
            _canRestart = true;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_canRestart && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
