using UnityEngine;
using SBR;

public class CarChannels : SBR.Channels {
    public CarChannels() {
        RegisterInputChannel("Gas", 0f, false);
        RegisterInputChannel("Steering", 0f, false);
        RegisterInputChannel("Rotation", new Vector3(0, 0, 0), false);

    }
    

    public float Gas {
        get {
            return GetInput<float>("Gas");
        }

        set {
            SetFloat("Gas", value);
        }
    }

    public float Steering {
        get {
            return GetInput<float>("Steering");
        }

        set {
            SetFloat("Steering", value);
        }
    }

    public Vector3 Rotation {
        get {
            return GetInput<Vector3>("Rotation");
        }

        set {
            SetVector("Rotation", value);
        }
    }

}
