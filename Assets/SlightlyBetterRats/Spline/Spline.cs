using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBR {
    public class Spline : MonoBehaviour {
        public SplineData spline;

        public Vector3 GetWorldPoint(float pos) {
            return transform.TransformPoint(spline.GetPoint(pos));
        }

        public Vector3 GetWoldTangent(float pos) {
            return transform.TransformVector(spline.GetTangent(pos));
        }

        public void GetWorldPoints(Vector3[] samples) {
            for (int i = 0; i < samples.Length; i++) {
                samples[i] = GetWorldPoint(i / (samples.Length - 1.0f));
            }
        }

        private void OnValidate() {
            UpdateMesh();
        }

        public void UpdateMesh() {
            spline.InvalidateSamples();

            var mesh = GetComponent<SplineMesh>();
            if (mesh) {
                mesh.UpdateMesh();
            }
        }
    }

}