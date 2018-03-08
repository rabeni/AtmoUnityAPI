using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingEmulator : MonoBehaviour {

    // for debug - to generate marker event with mouse click
    private TrackingHandler trackingHandler;
    private List<EmulatedMarker> existingMarkers = new List<EmulatedMarker>();
    private List<EmulatedMarker> hiddenMarkers = new List<EmulatedMarker>();

    public GameObject[] markerSprites = new GameObject[6];

    private int uniqueId = 0;
    private float markerRadius = 0.2395834f;

    public int _markerId;

	// Use this for initialization
	void Start () {
        trackingHandler = GameObject.Find("AtmoTracking").GetComponent<TrackingHandler>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            EmulatedMarker existingMarker = CheckCollisionWithExistingMarker(mousePosition);

            if (existingMarker == null)
                AddNewMarker(mousePosition, _markerId);

            else if (existingMarker.hidden)
                UnhideMarker(existingMarker);

            else
                HideMarker(existingMarker);
        }

        RemoveOverdueHiddenMarkers();
	}

    private EmulatedMarker CheckCollisionWithExistingMarker(Vector2 mousePosition)
    {
        EmulatedMarker existingMarker = null;

        // check collision with exsting marker
        for (int i = existingMarkers.Count - 1; i >= 0; i--)
        {
            // trigger onLost event if clicked in existing marker
            if (Vector2.Distance(existingMarkers[i].marker.position, mousePosition) < markerRadius)
            {
                existingMarker = existingMarkers[i];
            }
        }

        return existingMarker;
    }

    private void AddNewMarker(Vector3 mousePosition, int markerId)
    {
        EmulatedMarker newMarker = Instantiate(markerSprites[markerId], mousePosition, Quaternion.identity).GetComponent<EmulatedMarker>();
        newMarker.marker = new Marker(0, _markerId, uniqueId++, mousePosition);
        existingMarkers.Add(newMarker);
        trackingHandler.onDetectedEvent.Invoke(newMarker.marker);
    }

    private void RemoveMarker(EmulatedMarker markerToRemove)
    {
        existingMarkers.Remove(markerToRemove);
        markerToRemove.Remove();
    }

    private void HideMarker(EmulatedMarker marker)
    {
        trackingHandler.onLostEvent.Invoke(marker.marker);
        marker.Hide();
        hiddenMarkers.Add(marker);
    }

    private void UnhideMarker(EmulatedMarker hiddenMarker)
    {
        hiddenMarker.Unhide();
        hiddenMarkers.Remove(hiddenMarker);
        trackingHandler.onRedetectedEvent.Invoke(hiddenMarker.marker);
    }

    private void RemoveOverdueHiddenMarkers()
    {
        if (hiddenMarkers.Count > 0)
        {
            for (int i = hiddenMarkers.Count - 1; i >= 0;i--)
            {
                if (Time.time - hiddenMarkers[i].hideTimeStamp >= 5)
                {
                    RemoveMarker(hiddenMarkers[i]);
                    hiddenMarkers.Remove(hiddenMarkers[i]);
                }
            }
        }
    }


}
