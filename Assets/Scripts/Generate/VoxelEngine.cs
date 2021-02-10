using System;
using UnityEngine;

namespace Generate
{
    public class VoxelEngine : MonoBehaviour
    {
        private World _world = new World();
        private System.Random _random = new System.Random();
        public Material material;

        private void Start()
        {
            var chunkGameObject = new GameObject("Chunk 0, 0, 0");
            var chunk = chunkGameObject.AddComponent<Chunk>();
            _world.Chunks.Add(new ChunkId(0,0,0), chunk);
            // Set material
            chunkGameObject.GetComponent<MeshRenderer>().material = material;
        }

        private void Update()
        {
            var x = _random.Next(0, 16);
            var y = _random.Next(0, 16);
            var z = _random.Next(0, 16);
            var voxelType = (ushort) _random.Next(0, 2);

            _world[x, y, z] = voxelType;
        }
    }
}