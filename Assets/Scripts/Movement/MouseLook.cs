﻿using UnityEngine;

namespace Movement
{
    public class MouseLook : MonoBehaviour
    {

        public float mouseSensitivity = 100f;

        public Transform playerBody;
        public bool lockMouse;

        private float _xRotation;
    
        // Start is called before the first frame update
        private void Start()
        {
            Cursor.lockState = lockMouse ? CursorLockMode.Locked : CursorLockMode.None;
        }

        // Update is called once per frame
        private void Update()
        {
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
