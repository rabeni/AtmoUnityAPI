using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMarker : MonoBehaviour {

    public GameObject highlight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleOnDetected(int markerID, int uniqueID, Vector2 position)
    {
        switch (markerID)
        {
            case 0:
                AddHighlight(position, new Color32(0xF8, 0xB0, 0x68, 255), uniqueID);
                break;
            case 1:
                AddHighlight(position, new Color32(0xFF, 0x4F, 0x68, 255), uniqueID);
                break;
            case 2:
                AddHighlight(position, new Color32(0x39, 0x46, 0x4e, 255), uniqueID);
                break;
            case 3:
                AddHighlight(position, new Color32(0x6C, 0xE8, 0x90, 255), uniqueID);
                break;
            case 4:
                AddHighlight(position, new Color32(0x75, 0x61, 0xFF, 255), uniqueID);
                break;
            case 5:
                AddHighlight(position, new Color32(0x00, 0x61, 0xFF, 255), uniqueID);
                break;
            default:
                break;
        }
    }

    private void AddHighlight(Vector2 position, Color32 color, int id)
    {
        GameObject h = Instantiate(highlight);
        h.transform.position = position;
        h.GetComponent<SpriteRenderer>().color = color;
        h.name = id.ToString();
    }
}
