using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdSelectorMarker : MonoBehaviour {

    public bool selected = false;
    public SpriteRenderer marker;
	private IdSelector selector;

	// Use this for initialization
	void Start () 
	{
        marker = GetComponent<SpriteRenderer>();
		selector = GetComponentInParent<IdSelector> ();
	}

    // hover
    void OnMouseOver()
    {
        marker.color = new Color32(255, 0, 0, 255);
    }

    void OnMouseExit()
    {
        if (!selected)
            marker.color = new Color32(255, 255, 255, 255);
    }

    public void Select()
    {
        marker.color = new Color32(255, 0, 0, 255);
        selected = true;
    }

    public void Deselect()
    {
        marker.color = new Color32(255, 255, 255, 255);
        selected = false;
    }
}
