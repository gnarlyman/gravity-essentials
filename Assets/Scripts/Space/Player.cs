using System;
using UnityEngine;

namespace Space
{
    public class Player : MonoBehaviour
    {
        public Rigidbody rb;
        
        /// <summary>
        /// Speed scale for the velocity of the Rigidbody.
        /// </summary>
        public float speed;
        /// <summary>
        /// Rotation Speed scale for turning.
        /// </summary>
        public float rotationSpeed;
        
        // The vertical input from input devices.
        private float _vertical;
        // The horizontal input from input devices.
        private float _horizontal;
        
        public float horizontalSpeed = 2.0f;
        public float verticalSpeed = 2.0f;

        public Attractor attractor; 

        private void FixedUpdate()
        {
            _vertical = Input.GetAxis("Vertical");
            _horizontal = Input.GetAxis("Horizontal");

            var t = transform;
            var vVelocity = t.forward * (_vertical * speed * Time.fixedDeltaTime);
            var hVelocity = t.right * (_horizontal * speed * Time.fixedDeltaTime);
            rb.AddForce(vVelocity + hVelocity);

            var direction = rb.position - attractor.closest.rb.position;
            Quaternion rotation = Quaternion.FromToRotation(t.up, direction);
            t.rotation = rotation * t.rotation; 
            // transform.Rotate(transform.up * (_horizontal * rotationSpeed * Time.fixedDeltaTime));
        }

        private void Update()
        {
            // Get the mouse delta. This is not in the range -1...1
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y") * -1;

            transform.Rotate(v, h, 0);
        }
    }
}
