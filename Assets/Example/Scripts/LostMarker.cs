/*
  LostMarker.cs - This script handles the onLost event of Tracking Handler.
  It finds and removes markers on the scene based on their unique id.
  Created by Atmo, February 2, 2018.
*/

using UnityEngine;

public class LostMarker : MonoBehaviour {

    public void HandleOnLost(Marker marker)
    {
        Destroy(GameObject.Find(marker.uniqueID.ToString()));
    }
}
