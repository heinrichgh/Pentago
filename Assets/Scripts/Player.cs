using UnityEngine;

namespace Pentago
{
    public class Player
    {
        public GameObject PiecePrefab { get; }
        public int PlayerNumber { get; }

        public Player(GameObject piecePrefab, int playerNumber)
        {
            PiecePrefab = piecePrefab;
            PlayerNumber = playerNumber;
        }
    }
}