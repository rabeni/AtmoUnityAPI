using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmulatedMarker : MonoBehaviour {

    public Marker marker;
    public bool hidden = false;
    public float hideTimeStamp = 0;

    private SpriteRenderer diceFace;
    private TrackingEmulator trackingEmulator;

	// Use this for initialization
	void Start () {
        diceFace = GetComponent<SpriteRenderer>();

        trackingEmulator = transform.parent.GetComponent<TrackingEmulator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hide() 
    {
        hidden = true;
        hideTimeStamp = Time.time;

        diceFace.color = new Color(diceFace.color.r, diceFace.color.g, diceFace.color.b, 0.2f);
    }

    public void Unhide()
    {
        hidden = false;

        diceFace.color = new Color(diceFace.color.r, diceFace.color.g, diceFace.color.b, 1f);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

	private void OnMouseUp()
	{
        trackingEmulator.OnExistingClicked(this);
	}
}
