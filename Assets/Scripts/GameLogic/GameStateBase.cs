using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateBase : MonoBehaviour
{
    public abstract void ActivateState();

    protected void FinishState()
    {
        GameManager.Instance.NextState();
    }
}
