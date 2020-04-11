using UnityEngine;

namespace GESpace
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
        public float rotationRate = 0.1f;

        private float _pitch;
        public GameObject playerCamera;

        private void FixedUpdate()
        {
            _vertical = Input.GetAxis("Vertical");
            _horizontal = Input.GetAxis("Horizontal");

            var t = transform;
            var vVelocity = t.forward * (_vertical * speed * Time.fixedDeltaTime);
            var hVelocity = t.right * (_horizontal * speed * Time.fixedDeltaTime);
            rb.AddForce(vVelocity + hVelocity);

            var direction = rb.position - attractor.closest.rb.position;
            var rotation = Quaternion.FromToRotation(t.up, direction);
            // t.rotation = rotation * t.rotation;
            rb.rotation = Quaternion.Lerp(rb.rotation, rotation * t.rotation, rotationRate);
        }

        private void Update()
        {
            // Get the mouse delta. This is not in the range -1...1
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y") * -1;

            _pitch = Mathf.Clamp(_pitch + -v, -89, 89);
            playerCamera.transform.localRotation = Quaternion.Euler(_pitch, 0 ,0);

            transform.Rotate(0, h, 0);
        }
    }
}
