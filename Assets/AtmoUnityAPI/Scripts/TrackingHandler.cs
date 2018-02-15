using UnityEngine;
using System;
using UnityEngine.Events;

public class TrackingHandler : MonoBehaviour
{
    private enum CallbackType { Detected, Lost, ReDetected };

    private Transform Dice;
    public Camera MainCamera;

    [Serializable]
    public class CallbackEvent : UnityEvent<int, int, Vector2>
    {
    }
    public CallbackEvent onDetectedEvent;
    public CallbackEvent onRedetectedEvent;
    public CallbackEvent onLostEvent;

    void Start()
    {
        MainCamera = Camera.main;

        // Listner for new OSC data
        GetComponent<OSCController>().onNewOSCData.AddListener(ProcessOSCData);
    }

    // Invokes events based on types of dice data received via OSC
    private void ProcessOSCData(Marker newDice)
    {
        Vector2 worldPosition = GetWorldPosition(newDice);

        switch (newDice.eventType)
        {
            case (int)CallbackType.Detected:
                onDetectedEvent.Invoke(newDice.markerID, newDice.uniqueID, worldPosition);
                break;

            case (int)CallbackType.Lost:
                onLostEvent.Invoke(newDice.markerID, newDice.uniqueID, worldPosition);
                break;

            case (int)CallbackType.ReDetected:
                onRedetectedEvent.Invoke(newDice.markerID, newDice.uniqueID, worldPosition);
                break;

            default:
                break;
        }
    }

    // Converts camera position to Unity world position
    private Vector2 GetWorldPosition(Marker newDice)
    {
        Vector2 camPosition = new Vector2(newDice.camX, 800 - newDice.camY);
        return MainCamera.ScreenToWorldPoint(camPosition);
    }
}
