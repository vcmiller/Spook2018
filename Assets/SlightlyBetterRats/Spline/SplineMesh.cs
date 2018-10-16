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

        private MeshRenderer mr;
        private MeshFilter mf;
        private MeshCollider mc;

        private void OnValidate() {
            UpdateMesh();
        }

        public void UpdateMesh() {
            mr = GetComponent<MeshRenderer>();
            mf = GetComponent<MeshFilter>();
            mc = GetComponent<MeshCollider>();

            var m = mf.sharedMesh;
            if (m == null) {
                m = new Mesh();
                m.name = "Spline Mesh";
                mf.mesh = m;

                if (mc) mc.sharedMesh = m;
            }
            m.Clear();
            
            if (profile) {
                BuildMesh();
            }
        }

        private void BuildMesh() {

            var mesh = mf.sharedMesh;
            profile.AddMeshes(spline.spline, mf.sharedMesh);

            if (mc) mc.sharedMesh = mesh;
        }

        // Don't do anything in play mode.
        private void Start() {
            
        }
    }

}