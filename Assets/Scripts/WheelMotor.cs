using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class WheelMotor : BasicMotor<CarChannels> {
    public float hoverHitDistance = 2;
    public float hoverForce = 10;

    public float hoverMaxForce = 20;

    public float driveForce = 20;
    public float maxSpeed = 50;
    public bool doSteering;
    public float steerAngle = 30;

    public float antiDrifting = 5;

    private Rigidbody rb;
    private ParticleSystem[] emitters;
    private RaycastHit groundHit;
    private bool onGround;

    private Vector3 forcePoint {
        get {
            float comY = rb.centerOfMass.y;
            Vector3 localPos = transform.localPosition;
            localPos.y = comY;
            return transform.parent.TransformPoint(localPos);
        }
    }

    private Vector3 driveVector {
        get {
            if (doSteering) {
                Vector3 fwd = -transform.up;
                fwd = Quaternion.AngleAxis(steerAngle * channels.Steering, transform.forward) * fwd;
                return fwd;
            } else {
                return -transform.up;
            }
        }
    }

    private Vector3 driftVector {
        get {
            if (doSteering) {
                Vector3 right = transform.right;
                right = Quaternion.AngleAxis(steerAngle * channels.Steering, transform.forward) * right;
                return right;
            } else {
                return transform.right;
            }
        }
    }

    private bool enableEmitters {
        set {
            foreach (var em in emitters) {
                var module = em.emission;
                module.enabled = value;
            }
        }
    }

    protected override void Start() {
        base.Start();
        rb = GetComponentInParent<Rigidbody>();
        emitters = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update() {
        onGround = Physics.Raycast(transform.position, -transform.forward, out groundHit, hoverHitDistance);
        if (onGround) {
            float f = Mathf.Lerp(hoverMaxForce, hoverForce, groundHit.distance / hoverHitDistance);
            rb.AddForceAtPosition(transform.forward * f, forcePoint, ForceMode.Force);

            Vector3 driftVel = Vector3.Project(rb.velocity, driftVector);
            rb.AddForceAtPosition(-driftVel * antiDrifting, forcePoint, ForceMode.Force);
        }

        enableEmitters = onGround;
    }

    public override void TakeInput() {
        if (onGround) {
            Vector3 drive = driveVector;
            Vector3 driveVel = Vector3.Project(rb.velocity, drive);
            Vector3 targetVel = maxSpeed * channels.Gas * drive;

            rb.AddForceAtPosition((targetVel - driveVel) * driveForce, forcePoint, ForceMode.Force);
        }
    }
}
