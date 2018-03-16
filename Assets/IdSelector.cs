using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdSelector : MonoBehaviour {

    private IdSelectorMarker[] markers = new IdSelectorMarker[6];
    private int state = 0;
    private TrackingEmulator trackingEmulator;
    private int currentId = 0;

	// Use this for initialization
	void Start () {

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
            mousePosition.z = transform.position.z;
            Open(mousePosition);

            state = 1;
        }

        if (state == 1)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int id = CheckCollisionWithMarkerSelector(mousePosition);

            // if mouse is colliding with an IdSelectorMarker
            if (id != -1 && Input.GetMouseButtonUp(0))
            {
                markers[currentId].Deselect();
                markers[id].Select();
                currentId = id;
                trackingEmulator._markerId = id;
                Close();

                print("clicked");
            }
        }
    }

	


    private void Open(Vector3 center)
    {
        int resolution = 6;
        float radius = 0.7f;

        transform.position = center;

        for (int i = 0; i < resolution; i++)
        {
            float angle = i * Mathf.PI * 2 / resolution;
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
            //targetTransform.position = Vector3.Lerp(start, end, i/resolution);
            float x = EasingEquations.EaseInOutBack(start.x, end.x, i / resolution);
            float y = EasingEquations.EaseInOutBack(start.y, end.y, i / resolution);
            targetTransform.position = new Vector3(x, y, 0);

            yield return new WaitForSeconds(step);
        }
    }

    private void Close()
    {
        for (int i = 0; i < 6; i++)
        {
            markers[i].transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    private int CheckCollisionWithMarkerSelector(Vector2 mousePosition)
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
