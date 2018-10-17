using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class PlayerCar : MonoBehaviour {
    private Rigidbody rb;

    public float deadTime = 0.2f;
    public float deadSpeed = 0.001f;

    private ExpirationTimer deadTimer;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        deadTimer = new ExpirationTimer(deadTime);
	}
	
	// Update is called once per frame
	void Update () {
        if (rb.velocity.sqrMagnitude > deadSpeed * deadSpeed) {
            deadTimer.Set();
        }

		if (deadTimer.expired) {
            Vector3 e = transform.eulerAngles;
            e.z = e.x = 0;
            transform.eulerAngles = e;
        }
	}
}
