using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SBR {
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class SplineMesh : MonoBehaviour {
        public Spline spline;

        public SplineMeshProfile profile;

        private SplineMeshProfile _subscribedProfile;
        private SplineMeshProfile subscribedProfile {
            get {
                return _subscribedProfile;
            }
            set {
                if (_subscribedProfile) _subscribedProfile.PropertyChanged -= UpdateMesh;

                _subscribedProfile = value;

                if (_subscribedProfile) _subscribedProfile.PropertyChanged += UpdateMesh;
            }
        }

        private MeshRenderer mr;
        private MeshFilter mf;
        private MeshCollider mc;

        private void OnDisable() {
            subscribedProfile = null;
        }

        private void OnValidate() {
            if (!Application.isPlaying) {
                UpdateMesh();

                subscribedProfile = profile;
            }
        }

        public void UpdateMesh() {
            mr = GetComponent<MeshRenderer>();
            mf = GetComponent<MeshFilter>();
            mc = GetComponent<MeshCollider>();

            if (mf.sharedMesh) DestroyImmediate(mf.sharedMesh);
            if (mc && mc.sharedMesh) DestroyImmediate(mc.sharedMesh);

            if (profile && spline.spline.points.Length > 1) {
                Mesh mesh, collisionMesh;
                profile.CreateMeshes(spline.spline, out mesh, out collisionMesh);

                mf.sharedMesh = mesh;
                if (mc) mc.sharedMesh = collisionMesh;

                var sm = mr.sharedMaterials;
                int smc = profile.GetSubmeshCount();
                if (sm.Length != profile.GetSubmeshCount()) {
                    Array.Resize(ref sm, smc);
                    mr.sharedMaterials = sm;
                }
            }
        }
    }
}