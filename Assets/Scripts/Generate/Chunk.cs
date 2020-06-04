using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Generate
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        private readonly ushort[] _voxels = new ushort[16 * 16 * 16];
        private MeshFilter _meshFilter;
        private readonly Vector3[] _cubeVertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0, 1, 1),
            new Vector3 (1, 1, 1),
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1)
        };
        private readonly int[] _cubeTriangles = {
            // Front
            0, 2, 1,
            0, 3, 2,
            // Top
            2, 3, 4,
            2, 4, 5,
            // Right
            1, 2, 5,
            1, 5, 6,
            // Left
            0, 7, 4,
            0, 4, 3,
            // Back
            5, 4, 7,
            5, 7, 6,
            // Bottom
            0, 6, 7,
            0, 1, 6
        };
        
        private static readonly Vector3[] CubeNormals = {
            Vector3.up,Vector3.up,Vector3.up,
            Vector3.up,Vector3.up,Vector3.up,
            Vector3.up,Vector3.up
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