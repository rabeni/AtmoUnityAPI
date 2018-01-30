using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker 
{
    public int eventType;
    public int markerID;     
    public int uniqueID;   
    public int camX;    // Marker position in camera coordinates
    public int camY;    // Marker position in camera coordinates

    public Marker(int eventType, int diceType, int diceId, int diceCamX, int diceCamY)
    {
        this.eventType = eventType;
        this.markerID = diceType;
        this.uniqueID = diceId;
        this.camX = diceCamX;
        this.camY = diceCamY;
    }
}
