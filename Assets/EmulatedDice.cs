using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmulatedDice : MonoBehaviour {

    private int markerId;

	// Use this for initialization
	void Start () {
        markerId = int.Parse(gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
