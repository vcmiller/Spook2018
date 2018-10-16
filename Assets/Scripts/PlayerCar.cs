using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour {
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.IsSleeping()) {
            Vector3 e = transform.eulerAngles;
            e.z = e.x = 0;
            transform.eulerAngles = e;
        }
	}
}
