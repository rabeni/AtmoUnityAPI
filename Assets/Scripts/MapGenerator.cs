﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _hexPrefab;
    [SerializeField] private float _depth;

    private Transform _mapParent;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        _mapParent = new GameObject("MapParent").transform;

        

        int horizontalIterCnt, verticalIterCnt;
        GetIterationCount(out horizontalIterCnt, out verticalIterCnt);

        var lowerLeftScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        var hexHeight = _hexPrefab.GetComponent<SpriteRenderer>().size.y * _hexPrefab.transform.lossyScale.y;
        var hexWidth = _hexPrefab.GetComponent<SpriteRenderer>().size.x * _hexPrefab.transform.lossyScale.x;
        var col = new GameObject("Col").transform;
        col.SetParent(_mapParent);
        col.position = lowerLeftScreen + verticalIterCnt / 2 * Vector3.up;

        for (int y = 0; y < verticalIterCnt; y++)
        {
            Instantiate(_hexPrefab, lowerLeftScreen + Vector3.up * y * hexHeight, Quaternion.identity,
                col);
        }

        Vector3 downDisplacement = new Vector3(hexWidth*0.75f, -hexHeight*0.5f, 0f);
        Vector3 upDisplacement = new Vector3(hexWidth*0.75f, hexHeight*0.5f, 0f);

        Vector3 lastRowPosition = col.transform.position;
        for (int x = 1; x < horizontalIterCnt; x++)
        {
            lastRowPosition = Instantiate(col, lastRowPosition + (x % 2 == 1 ? downDisplacement : upDisplacement), Quaternion.identity, _mapParent).transform.position;
        }
    }

    private void GetIterationCount(out int horizontalIterCnt, out int verticalIterCnt)
    {
        var lowerLeftScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        var upperLeftScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z));
        var lowerRightScreen = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -Camera.main.transform.position.z));
        var hexHeight = _hexPrefab.GetComponent<SpriteRenderer>().size.y * _hexPrefab.transform.lossyScale.y;
        var hexWidth = _hexPrefab.GetComponent<SpriteRenderer>().size.x * _hexPrefab.transform.lossyScale.x;

        horizontalIterCnt = (int)((Vector3.Distance(lowerLeftScreen, lowerRightScreen) / (hexWidth*1.5)))*2+4;
        verticalIterCnt = (int)(Vector3.Distance(lowerLeftScreen, upperLeftScreen) / (hexHeight)+2);
    }
}
