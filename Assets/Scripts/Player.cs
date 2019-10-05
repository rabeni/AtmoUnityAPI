using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public enum PlayerColor
    {
        RED=0,
        GREEN=1,
        BLUE=2,
        YELLOW=3
    }

    private PlayerColor _playerColor;

    public Player(PlayerColor color)
    {
        _playerColor = color;
    }
}
