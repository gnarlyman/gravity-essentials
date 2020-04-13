using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement: MonoBehaviour
    {
        [Header("Walk / Run Setting")] 
        public float walkSpeed;
        public float runSpeed;
        
        [Header("Jump Settings")] 
        public float playerJumpForce;
        public ForceMode appliedForceMode;
        
        [Header("Player State")] 
        public bool playerIsJumping;
        public bool playerIsSprinting;
        public float currentSpeed;
        
        [Header("Player Rotation")]
        public GESpace.Attractor attractor;
        public float rotationRate;

        [Header("Debug")] 
        public float distanceToGround;
        

        private float _inputXAxis;
        private float _inputZAxis;
        private Rigidbody _rb;
        private RaycastHit _hit;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // Collect Player Input
            _inputXAxis = Input.GetAxis("Horizontal");
            _inputZAxis = Input.GetAxis("Vertical");
            playerIsJumping = Input.GetButton("Jump");
            playerIsSprinting = Input.GetKey(KeyCode.LeftShift);
            
            // Player Sprint/Run
            currentSpeed = playerIsSprinting ? runSpeed : walkSpeed;
            
            // Detect Ground
            var tPos = transform.position;
            var directionDown = transform.TransformDirection(Vector3.down);
            // Debug.DrawRay(tPos,directionDown * 10f, Color.white);
            var hitGround = Physics.Raycast(tPos, directionDown, 
                out _hit, 10f, 1 << 8);
            if (hitGround)
            {
                // If Ground found and player is close enough
                distanceToGround = Vector3.Distance(tPos, _hit.point);
                if (distanceToGround > 2f)
                {
                    // player is not jumping
                    playerIsJumping = false;
                }

                // Rotate Player to the hit object's up vector
                // RotatePlayerToObjectUp();
            }
        // }
        // private void FixedUpdate()
        // {
            // Move Player
            // var tPos = transform.position;
            _rb.MovePosition(tPos + Time.deltaTime * currentSpeed * 
                transform.TransformDirection(_inputXAxis, 0f, _inputZAxis));

            if (playerIsJumping)
                PlayerJump(playerJumpForce, appliedForceMode);
            
            // If no ground, rotate Player toward the nearest Attractor
            RotatePlayerToAttractor();
        }

        private void PlayerJump(float jumpForce, ForceMode forceMode)
        {
            _rb.AddForce(jumpForce * _rb.mass * Time.deltaTime * Vector3.up, forceMode);
        }

        private void RotatePlayerToObjectUp()
        // Rotate the body to stay upright
        // When raycast collides with mask
        {
            var t = transform;
            var tRot = t.rotation;
            var rotation = Quaternion.FromToRotation(t.up, _hit.normal);
            _rb.rotation = Quaternion.Lerp(tRot, rotation, rotationRate);
        }

        private void RotatePlayerToAttractor()
        // Rotate the body to stay upright above Attractor
        {
            var t = transform;
            var tPos = t.position;
            var tRot = t.rotation;
            var direction =  tPos - attractor.closest.transform.position;

            var gravityForward = Vector3.Cross(-direction.normalized, t.right);
            var targetRotation = Quaternion.LookRotation(gravityForward, direction.normalized);
            _rb.rotation = Quaternion.Lerp(tRot, targetRotation, rotationRate);
        }
    }
}