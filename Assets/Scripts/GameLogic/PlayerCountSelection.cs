using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountSelection : GameStateBase
{
    [SerializeField] private GameObject _playerCountSelectionPanel;

    public override void ActivateState()
    {
        _playerCountSelectionPanel.SetActive(true);
    }

    public void SetPlayerCount(int playerCount)
    {
        _playerCountSelectionPanel.SetActive(false);
        GameManager.Instance.CreatePlayers(playerCount);
        FinishState();
    }
}
