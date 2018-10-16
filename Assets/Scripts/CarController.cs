using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class CarController : PlayerController {
    private new CarChannels channels;

    public float mouseSensitivity = 1;

    public float minPitch = -45;
    public float maxPitch = 45;

    public bool invertMouseX;
    public bool invertMouseY;

    private float invertXMul => invertMouseX ? -1 : 1;
    private float invertYMul => invertMouseY ? -1 : 1;

    public override void Initialize() {
        base.Initialize();
        channels = base.channels as CarChannels;
    }

    public void Axis_Vertical(float value) {
        channels.Gas = value;
    }

    public void Axis_Horizontal(float value) {
        channels.Steering = value;
    }

    public void Axis_MouseX(float value) {
        Vector3 rot = channels.Rotation;
        rot.y += value * mouseSensitivity * invertXMul;
        channels.Rotation = rot;
    }

    public void Axis_MouseY(float value) {
        Vector3 rot = channels.Rotation;
        rot.x = Mathf.Clamp(rot.x + value * mouseSensitivity * invertYMul, minPitch, maxPitch);
        channels.Rotation = rot;
    }
}
