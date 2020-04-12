using System;
using UnityEngine;

namespace Movement
{
    public class PlayerCamera : MonoBehaviour
    {
        public Transform playerFace;
        public float rotationRate;
        
        private void FixedUpdate()
        {
            transform.position = playerFace.position;
            transform.rotation = playerFace.rotation;
            
        }
    }
}