using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingEmulator : MonoBehaviour {

    // for debug - to generate marker event with mouse click
    private TrackingHandler trackingHandler;
    private List<Marker> _markers = new List<Marker>();

    private int uniqueId = 0;
    private float markerRadius = 0.2395834f;

    public int markerId;

	// Use this for initialization
	void Start () {
        // for debug - to generate marker event with mouse click
        trackingHandler = GameObject.Find("AtmoTracking").GetComponent<TrackingHandler>();
	}
	
	// Update is called once per frame
	void Update () {
        bool isLostEvent = false;

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // check collision with exsting marker
            for (int i = _markers.Count - 1; i >= 0; i--)
            {
                // trigger onLost event if clicked in existing marker
                if (Vector2.Distance(_markers[i].position, mousePosition) < markerRadius)
                {
                    trackingHandler.onLostEvent.Invoke(_markers[i]);
                    _markers.Remove(_markers[i]);
                    isLostEvent = true;
                }
            }

            // if there's no existing marker on the mouse position trigger onDetected event
            if (!isLostEvent)
            {
                Marker newMarker = new Marker(0, markerId, uniqueId++, mousePosition);
                trackingHandler.onDetectedEvent.Invoke(newMarker);
                _markers.Add(newMarker);
            }
        }
	}
}
