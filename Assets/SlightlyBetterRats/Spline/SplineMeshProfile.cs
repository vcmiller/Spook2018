using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBR {
    [CreateAssetMenu(fileName = "NewProfile", menuName = "Spline Mesh Profile")]
    public class SplineMeshProfile : ScriptableObject {
        public bool separateCollisionMesh;

        [System.Serializable]
        public class MeshInfo {
            public Mesh render;
            public Mesh collision;
            public int repeat;
            public bool allowStretch;
        }

        public MeshInfo[] meshes;

        public virtual void AddMeshes(SplineData spline, Mesh mesh) {
            if (meshes.Length == 0) {
                Debug.LogError("SplineMeshProfile needs at least one mesh!");
            }
            
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();

            float f = 0;
            int meshIndex = 0;
            int rep = 0;

            float stretch = CalculateStretch(spline);

            while (f < 1) {
                f = AddMesh(spline, meshIndex, stretch, f, vertices, normals, uvs, triangles);

                rep++;
                if (rep >= meshes[meshIndex].repeat) {
                    rep = 0;
                    meshIndex = (meshIndex + 1) % meshes.Length;
                }
            }
            
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);

            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
        }

        protected virtual float CalculateStretch(SplineData spline) {
            float length = spline.length;

            float meshLength = 0;
            float stretchable = 0;

            int meshIndex = 0;
            int rep = 0;
            while (true) {
                float sz = meshes[meshIndex].render.bounds.size.z;

                if (meshLength + sz <= length) {
                    meshLength += sz;

                    if (meshes[meshIndex].allowStretch) {
                        stretchable += sz;
                    }
                } else {
                    break;
                }

                rep++;
                if (rep >= meshes[meshIndex].repeat) {
                    rep = 0;
                    meshIndex = (meshIndex + 1) % meshes.Length;
                }
            }

            if (stretchable > 0) {
                return (length - meshLength + stretchable) / stretchable;
            } else {
                return 1;
            }
        }

        protected virtual float AddMesh(SplineData spline, int index, float stretch, float splineOffset, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs, List<int> triangles) {
            var toAdd = meshes[index].render;
            var bounds = toAdd.bounds;

            bool str = meshes[index].allowStretch;
            float sizeZ = bounds.size.z;
            if (str) {
                sizeZ *= stretch;
            }

            float maxPos = splineOffset + sizeZ / spline.length;
            int vCount = vertices.Count;

            if (maxPos <= 1 + (0.1f / spline.length)) {
                Vector3[] inVerts = toAdd.vertices;
                Vector3[] inNormals = toAdd.normals;
                Vector2[] inUvs = toAdd.uv;

                for (int i = 0; i < inVerts.Length; i++) {
                    var vert = inVerts[i];
                    var normal = inNormals[i];
                    var uv = inUvs[i];

                    vert.z -= bounds.min.z;
                    if (str) {
                        vert.z *= stretch;
                    }
                    float pos = Mathf.Clamp01(splineOffset + (vert.z / spline.length));
                    var srot = spline.GetRotation(pos);
                    var spos = spline.GetPoint(pos);

                    vert = spos + (srot * Vector3.ProjectOnPlane(vert, Vector3.forward));
                    normal = srot * normal;
                    vertices.Add(vert);
                    normals.Add(normal);
                    uvs.Add(uv);
                }
            
                triangles.AddRange(toAdd.GetTriangles(0).Select(i => i + vCount));
            }
            
            return maxPos;
        }
    }

}