using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker 
{
    public int eventType;
    public int markerID;     
    public int uniqueID;
    public Vector2 position;

    public Marker(int eventType, int markerId, int uniqueId, Vector2 worldPosition)
    {
        this.eventType = eventType;
        this.markerID = markerId;
        this.uniqueID = uniqueId;
        this.position = worldPosition;
    }
}
