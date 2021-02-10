using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;

namespace Generate
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        private readonly ushort[] _voxels = new ushort[16 * 16 * 16];
        private MeshFilter _meshFilter;
        static Vector3 p0 = new Vector3(0, 0, 0);
        static Vector3 p1 = new Vector3(1, 0, 0);
        static Vector3 p2 = new Vector3(1, 1, 0);
        static Vector3 p3 = new Vector3(0, 1, 0);
        static Vector3 p4 = new Vector3(0, 1, 1);
        static Vector3 p5 = new Vector3(1, 1, 1);
        static Vector3 p6 = new Vector3(1, 0, 1);
        static Vector3 p7 = new Vector3(0, 0, 1);
        
        private readonly Vector3[] _cubeVertices = {
            // Bottom
            p0, p1, p6, p7,
            
            // Left
            p0, p1, p2, p3,
            
            // Front
            p0, p7, p4, p3,
            
            // Back
            p1, p6, p5, p2,
            
            // Right
            p7, p6, p5, p4,
            
            // Top
            p3, p2, p5, p4
        };
        private readonly int[] _cubeTriangles = {
            // Bottom
            0, 1, 2,
            0, 2, 3,
            
            // Left
            0 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
            0 + 4 * 1, 3 + 4 * 1, 2 + 4 * 1,
            
            // Front
            0 + 4 * 2, 1 + 4 * 2, 2 + 4 * 2,
            0 + 4 * 2, 2 + 4 * 2, 3 + 4 * 2,

            // Back
            0 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
            0 + 4 * 3, 3 + 4 * 3, 2 + 4 * 3,
            
            // Right
            0 + 4 * 4, 1 + 4 * 4, 2 + 4 * 4,
            0 + 4 * 4, 2 + 4 * 4, 3 + 4 * 4,
            
            // Top
            0 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
            0 + 4 * 5, 3 + 4 * 5, 2 + 4 * 5
        };

        static Vector3 up = Vector3.up;
        static Vector3 down = Vector3.down;
        static Vector3 front = Vector3.forward;
        static Vector3 back = Vector3.back;
        static Vector3 left = Vector3.left;
        static Vector3 right = Vector3.right;
        
        private static readonly Vector3[] CubeNormals = {
            // Bottom
            down, down, down, down,
            
            // Left
            left, left, left, left,
            
            // Front
            front, front, front, front,
            
            // Back
            back, back, back, back,
            
            // Right
            right, right, right, right,
            
            // Top
            up, up, up, up
        };

        public ushort this[int x, int y, int z]
        {
            get => _voxels[x * 16 * 16 + y * 16 + z];
            set => _voxels[x * 16 * 16 + y * 16 + z] = value;
        }

        void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void Update()
        {
            RenderToMesh();
        }

        private void RenderToMesh()
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();

            for (var x = 0; x < 16; x++)
            {
                for (var y = 0; y < 16; y++)
                {
                    for (var z = 0; z < 16; z++)
                    {
                        var voxelType = this[x, y, z];
                        if (voxelType == 0)
                            continue;
                        var pos = new Vector3(x, y, z);
                        var verticesPos = vertices.Count;
                        vertices.AddRange(_cubeVertices.Select(vert => pos + vert));
                        triangles.AddRange(_cubeTriangles.Select(tri => verticesPos + tri));
                        normals.AddRange(CubeNormals);
                    }
                }
            }
            
            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.SetNormals(normals);

            _meshFilter.mesh = mesh;
        }
    }
    
}