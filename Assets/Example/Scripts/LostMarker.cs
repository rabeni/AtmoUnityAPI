using UnityEngine;

public class LostMarker : MonoBehaviour {

    public void HandleOnLost(Marker marker)
    {
        Destroy(GameObject.Find(marker.uniqueID.ToString()));
    }
}
