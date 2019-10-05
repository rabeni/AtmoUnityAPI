using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private enum GameState
    {
        PlayerCountSelection=0,
        PlayerOrderSelection=1,
        CharacterSelection=2,
        Game=3,
        End=4
    }

    public int PlayerCount
    {
        get { return Players.Count; }
    }

    public List<Player> Players;

    [SerializeField] List<GameStateBase> _gameStates = new List<GameStateBase>();

    private GameState _currentState;

    public void CreatePlayers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Players.Add(new Player((Player.PlayerColor)count));
        }
    }

    public void NextState()
    {
        _currentState++;
        ActivateState();
    }

    private void Start()
    {
        ActivateState();
    }

    private void ActivateState()
    {
        _gameStates[(int)_currentState].ActivateState();
    }
}
