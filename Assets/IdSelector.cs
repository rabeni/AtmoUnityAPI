using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdSelector : MonoBehaviour {

    private IdSelectorMarker[] markers = new IdSelectorMarker[6];
    private int state = 0;
    private TrackingEmulator trackingEmulator;
    private int currentId = 0;

    public float selectorZ = -2f;
    public float selectorMarkerZ = -3f;

	// Use this for initialization
	void Start () {

        // set z postition to separate IdSelector clicks from other clicks
        transform.position = new Vector3(transform.position.x, transform.position.y, selectorZ);

        trackingEmulator = transform.parent.GetComponent<TrackingEmulator>();

		// Disable boxcollider while selector is closed
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;

		// Get IdSelectorMarkers
        for (int i = 0; i < transform.childCount; i++)
        {
            markers[i] = transform.GetChild(i).GetComponent<IdSelectorMarker>();
        }

		// Select default chosen id
        markers[currentId].Select();

	}

    void Update()
    {

        if (Input.GetMouseButtonUp(1))
        {
            Close();

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = selectorZ;
            Open(mousePosition);

            state = 1;
        }

        if (state == 1)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			// with different z, there's no match
			mousePosition.z = selectorMarkerZ;
            int id = CheckCollisionWithMarkerSelector(mousePosition);

            // if mouse is colliding with an IdSelectorMarker
            if (id != -1 && Input.GetMouseButtonUp(0))
            {
                markers[currentId].Deselect();
                markers[id].Select();
                currentId = id;
                trackingEmulator._markerId = id;
                Close();
            }
        }
    }

	// if selector is open and there's a click, close it
	void OnMouseUp() 
	{
		Close ();
	}

    private void Open(Vector3 center)
    {
        int numOfItems = 6;
        float radius = 0.6f;

        for (int i = 0; i < numOfItems; i++)
        {
            float angle = i * Mathf.PI * 2 / numOfItems;
			Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + center;

			StartCoroutine(Translate_c(markers[i].transform, center, pos));
        }

		// enable selector layer collider
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
    }

	private IEnumerator Translate_c(Transform targetTransform, Vector3 start, Vector3 end)
    {
        float duration = 0.2f;
        float resolution = duration * 30f; //30 FPS

        float step = duration / resolution;

        for (int i = 1; i < resolution + 1; i++)
        {
            float x = EasingEquations.EaseOutCubic(start.x, end.x, i / resolution);
            float y = EasingEquations.EaseOutCubic(start.y, end.y, i / resolution);
			targetTransform.position = new Vector3 (x, y, selectorMarkerZ);//targetTransform.position.z);

            float scale = EasingEquations.EaseOutCubic(0, 1, i / resolution);
            targetTransform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForSeconds(step);
        }
    }

    private void Close()
    {
        for (int i = 0; i < 6; i++)
        {
			markers[i].transform.localPosition = new Vector3(0, 0, selectorMarkerZ);

            markers[i].transform.localScale = new Vector3(0, 0, 0);
        }

		// disable selector layer collider
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;
    }

    private int CheckCollisionWithMarkerSelector(Vector3 mousePosition)
    {
        int index = -1;

        for (int i = 0; i < 6; i++)
        {
            if (markers[i].transform.GetComponent<Collider2D>().bounds.Contains(mousePosition))
            {
                index = i;
            }
        }

        return index;
    }
}
