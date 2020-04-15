using System;
using UnityEngine;

namespace Movement
{
    public class PlayerCamera : MonoBehaviour
    {
        public Transform playerFace;
        public float rotationRate;
        
        private void LateUpdate()
        {
            var transform1 = transform;
            transform1.position = playerFace.position;
            transform1.rotation = playerFace.rotation;
        }
    }
}