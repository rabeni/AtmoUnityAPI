using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingEmulator : MonoBehaviour {

    private TrackingHandler trackingHandler;
    private List<EmulatedMarker> existingMarkers = new List<EmulatedMarker>();
    private List<EmulatedMarker> hiddenMarkers = new List<EmulatedMarker>();

    public GameObject[] markerSprites = new GameObject[6];

    private int uniqueId = 0;

    public int _markerId;

	void Start () 
	{
		try 
		{
			trackingHandler = GameObject.Find("AtmoTracking").GetComponent<TrackingHandler>();
		}
		catch (System.Exception ex) 
		{
			Debug.LogError ("AtmoTracking gameObject is missing.");
		}

		StartCoroutine (RemoveOverdueHiddenMarkers ());
	}

    // Add new marker on left click
	private void OnMouseUp()
	{
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // between the emulator collider and IdSelector
        mousePosition.z = -1;

        AddNewMarker(mousePosition, _markerId);

    }

	public void OnExistingClicked(EmulatedMarker clickedMarker)
	{
		if (clickedMarker.hidden)
			UnhideMarker(clickedMarker);

		else
			HideMarker(clickedMarker);
	}

	private bool CheckCollisionWithEmulator(Vector3 mousePosition)
    {
        return transform.GetComponent<Collider2D>().bounds.Contains(mousePosition);
    }

	private EmulatedMarker CheckCollisionWithExistingMarker(Vector3 mousePosition)
    {
        EmulatedMarker existingMarker = null;

        // check collision with exsting marker
        for (int i = existingMarkers.Count - 1; i >= 0; i--)
        {
            // trigger onLost event if clicked in existing marker
            if (existingMarkers[i].transform.GetComponent<Collider2D>().bounds.Contains(mousePosition))
            {
                existingMarker = existingMarkers[i];
            }
        }

        return existingMarker;
    }

    private void AddNewMarker(Vector3 mousePosition, int markerId)
    {
        EmulatedMarker newMarker = Instantiate(markerSprites[markerId], mousePosition, Quaternion.identity, transform).GetComponent<EmulatedMarker>();
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

	// check and remove hidden markers every sec
	IEnumerator RemoveOverdueHiddenMarkers()
	{
		while (true) {
			yield return new WaitForSeconds (1);
			if (hiddenMarkers.Count > 0) {
				for (int i = hiddenMarkers.Count - 1; i >= 0; i--) {
					if (Time.time - hiddenMarkers [i].hideTimeStamp >= 5) {
						RemoveMarker (hiddenMarkers [i]);
						hiddenMarkers.Remove (hiddenMarkers [i]);
					}
				}
			}
		}
	}
}
