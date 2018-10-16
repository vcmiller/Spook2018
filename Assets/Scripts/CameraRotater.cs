using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class CameraRotater : BasicMotor<CarChannels> {
    public override void TakeInput() {
        transform.eulerAngles = channels.Rotation;
    }
}
