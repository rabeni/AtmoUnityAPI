using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdSelector : MonoBehaviour {

    private IdSelectorMarker[] markers = new IdSelectorMarker[6];
    private int state = 0;
    private TrackingEmulator trackingEmulator;
    private int currentId = 0;

    public float z = -2f;

	// Use this for initialization
	void Start () {

        // set z postition to separate IdSelector clicks from other clicks
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        trackingEmulator = transform.parent.GetComponent<TrackingEmulator>();

        for (int i = 0; i < transform.childCount; i++)
        {
            markers[i] = transform.GetChild(i).GetComponent<IdSelectorMarker>();
        }

        markers[currentId].Select();

	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(1))
        {
            Close();

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = z;
            Open(mousePosition);

            state = 1;
        }

        if (state == 1)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = z;
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

    private void Open(Vector3 center)
    {
        int numOfItems = 6;
        float radius = 0.6f;

        transform.position = center;

        for (int i = 0; i < numOfItems; i++)
        {
            float angle = i * Mathf.PI * 2 / numOfItems;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius + center;

            //markers[i].position = pos;
            StartCoroutine(Translate_c(markers[i].transform, pos));
        }
    }

    private IEnumerator Translate_c(Transform targetTransform, Vector3 end)
    {
        float duration = 0.5f;
        float resolution = duration * 30f; //30 FPS

        float step = duration / resolution;
        Vector3 start = targetTransform.position;

        for (int i = 1; i < resolution + 1; i++)
        {
            float x = EasingEquations.EaseInOutBack(start.x, end.x, i / resolution);
            float y = EasingEquations.EaseInOutBack(start.y, end.y, i / resolution);
            targetTransform.position = new Vector3(x, y, targetTransform.position.z);

            float scale = EasingEquations.EaseInOutBack(0, 1, i / resolution);
            targetTransform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForSeconds(step);
        }
    }

    private void Close()
    {
        for (int i = 0; i < 6; i++)
        {
            markers[i].transform.localPosition = new Vector3(0, 0, 0);

            markers[i].transform.localScale = new Vector3(0, 0, 0);
        }
    }

    private int CheckCollisionWithMarkerSelector(Vector3 mousePosition)
    {
        int index = -1;

        // check collision with exsting marker
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
