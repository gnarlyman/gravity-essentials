using System;
using UnityEngine;

/* -------------------------------------------------------------------------------
	GravityFPSWalker
 
	This component is added to a GameObject with a RigidBody. It allows the player
	to move the RigidBody using the vertical and horizontal inputs, and to jump
	using the jump button.
 
	The RigidBody is pushed towards its own custom Gravity vector. The body will
	rotate to stay upright with the RotationRate.
 
	This component uses a raycast to determine if the RigidBody is standing on 
	the ground. The GroundHeight variable should be the distance between the
	GameObject transform and a little further than the bottom of the RigidBody.
 
	The LookTransform should be a child GameObject which points in the direction
	that the player is looking at. This could for example be a child GameObject 
	with a camera. The LookTransform is used to determine the movement veectors.
 ------------------------------------------------------------------------------ */
namespace GESpace
{
	[RequireComponent(typeof(Rigidbody))]
	public class GravityFpsWalker : MonoBehaviour {
 
		public GameObject playerCamera;
		// public Vector3 gravity = Vector3.down * 9.81f;
		public float rotationRate = 0.1f;
		public float velocity = 8;
		public float groundControl = 1.0f;
		public float airControl = 0.2f;
		public float jumpVelocity = 5;
		public float groundHeight = 1.1f;
		private bool _jump;
		private Rigidbody _rigidBody;
		
		public float horizontalSpeed = 2.0f;
		public float verticalSpeed = 2.0f;

		public Attractor attractor;
		
		private float _pitch;
		public float speed;

		private void Start()
		{
			_rigidBody = GetComponent<Rigidbody>();
			_rigidBody.freezeRotation = true;
			_rigidBody.useGravity = false;
		}

		void FixedUpdate() {
			
			// Get the mouse delta. This is not in the range -1...1
			var h = horizontalSpeed * Input.GetAxis("Mouse X");
			var v = verticalSpeed * Input.GetAxis("Mouse Y");

			_pitch = Mathf.Clamp(_pitch + -v, -91, 91);
			playerCamera.transform.localRotation = Quaternion.Euler(_pitch, 0 ,0);

			transform.Rotate(0, h, 0); 
		
			var direction = _rigidBody.position - attractor.closest.rb.position;
			
			// Cast a ray towards the ground to see if the Walker is grounded
			var grounded = Physics.Raycast(transform.position, direction.normalized, groundHeight);
 
			// Rotate the body to stay upright
			var gravityForward = Vector3.Cross(-direction.normalized, transform.right);
			var targetRotation = Quaternion.LookRotation(gravityForward, direction.normalized);
			_rigidBody.rotation = Quaternion.Lerp(_rigidBody.rotation, targetRotation, rotationRate);
 		
			// Add velocity change for movement on the local horizontal plane
			var vertical = Input.GetAxis("Vertical");
			var horizontal = Input.GetAxis("Horizontal");
			
			if (grounded)
			{
				
			}
			else
			{
				var t = playerCamera.transform;
				var vVelocity = t.forward * (vertical * speed * Time.fixedDeltaTime);
				var hVelocity = t.right * (horizontal * speed * Time.fixedDeltaTime);
				_rigidBody.AddForce(vVelocity + hVelocity);
			}

 		
			// Add gravity
			// _rigidBody.AddForce(gravity * _rigidBody.mass);
 
			_jump = false;
		}

		private void Update()
		{
			_jump = _jump || Input.GetButtonDown("Jump");
		}
	}
}