/*
  Marker.cs - Marker class implementation.
  Created by Atmo, February 2, 2018.
*/

using UnityEngine;

public class Marker 
{
    public int eventType;
    /// <summary>Id of the marker, same for identical markers.</summary>
    public int markerID;  
    /// <summary>Unique id of detection, remains the same for all three events invoked by the same detection.</summary>
    public int uniqueID;
    /// <summary>postion of the marker in Unity world space.</summary>
    public Vector2 position;

    public Marker(int eventType, int markerId, int uniqueId, Vector2 worldPosition)
    {
        this.eventType = eventType;
        this.markerID = markerId;
        this.uniqueID = uniqueId;
        this.position = worldPosition;
    }
}
