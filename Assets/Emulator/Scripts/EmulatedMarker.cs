using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmulatedMarker : MonoBehaviour {

    public Marker marker;
    public bool hidden = false;
    public float hideTimeStamp = 0;

    private Sprite diceFace;

	// Use this for initialization
	void Start () {
        diceFace = GetComponent<Sprite>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hide() 
    {
        hidden = true;
        hideTimeStamp = Time.time;
    }

    public void Unhide()
    {
        hidden = false;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
