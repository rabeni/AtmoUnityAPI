using UnityEngine;
using System;
using UnityEngine.Events;

public class TrackingHandler : MonoBehaviour
{
    private enum CallbackType { Detected, Lost, ReDetected };

    private Transform Dice;

    [Serializable]
    public class CallbackEvent : UnityEvent<Marker>
    {
    }
    public CallbackEvent onDetectedEvent;
    public CallbackEvent onRedetectedEvent;
    public CallbackEvent onLostEvent;

    void Start()
    {
        // Listner for new marker data via OSC
        GetComponent<OSCController>().onNewOSCData.AddListener(InvokeMarkerEvents);
    }

    // Invokes events based on type of marker data received
    private void InvokeMarkerEvents(Marker newMarker)
    {
        switch (newMarker.eventType)
        {
            case (int)CallbackType.Detected:
                onDetectedEvent.Invoke(newMarker);
                break;

            case (int)CallbackType.Lost:
                onLostEvent.Invoke(newMarker);
                break;

            case (int)CallbackType.ReDetected:
                onRedetectedEvent.Invoke(newMarker);
                break;

            default:
                break;
        }
    }
}
