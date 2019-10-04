using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdSelector : MonoBehaviour {

    private IdSelectorMarker[] markers = new IdSelectorMarker[6];
    private bool open = false;
    private TrackingEmulator trackingEmulator;
    private int currentId = 0;

	private IdSelectorMarker current;
	private IdSelectorMarker prev;


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
		// right click to open selector
        if (Input.GetMouseButtonUp(1))
        {
            Open();
        }
			
		// left click on marker selector to select marker
		if (open == true && Input.GetMouseButtonUp(0))
        {
            int id = CheckCollisionWithMarkerSelector();

            // if mouse is colliding with an IdSelectorMarker
            if (id != -1)
            {
				StartCoroutine(OnMarkerSelected (id));
            }
        }
    }

	public IEnumerator OnMarkerSelected(int id)
	{
		markers[currentId].Deselect();
		markers[id].Select();
		currentId = id;
		trackingEmulator._markerId = id;
		yield return new WaitForSeconds (0.2f);
		Close();
	}

	// if selector is open and there's a click, close it
	void OnMouseUp() 
	{
		Close ();
	}

    private void Open()
    {
        int numOfItems = 6;
        float radius = 0.6f;
		open = true;

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = selectorZ;

        for (int i = 0; i < numOfItems; i++)
        {
            float angle = i * Mathf.PI * 2 / numOfItems;
			Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + mousePosition;

			StartCoroutine(OpenAnim_c(markers[i].transform, mousePosition, pos, 0, 1));
        }

		// enable selector layer collider
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
    }

	private IEnumerator OpenAnim_c(Transform targetTransform, Vector3 start, Vector3 end, float startScale, float endScale)
    {
        float duration = 0.2f;
        float resolution = duration * 30f; //30 FPS

        float step = duration / resolution;

        for (int i = 1; i < resolution + 1; i++)
        {
            float x = EasingEquations.EaseOutCubic(start.x, end.x, i / resolution);
            float y = EasingEquations.EaseOutCubic(start.y, end.y, i / resolution);
			targetTransform.position = new Vector3 (x, y, selectorMarkerZ);//targetTransform.position.z);

			float scale = EasingEquations.EaseOutCubic(startScale, endScale, i / resolution);
			targetTransform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForSeconds(step);
        }
    }

    private void Close()
    {
		open = false;

		for (int i = 0; i < 6; i++)
		{
			markers[i].transform.localPosition = new Vector3(0, 0, selectorMarkerZ);
			markers[i].transform.localScale = new Vector3(0, 0, 0);
		}

		// disable selector layer collider
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;
    }

    private int CheckCollisionWithMarkerSelector()
    {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = selectorMarkerZ;

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
