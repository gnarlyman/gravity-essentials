using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12f;
    
    // Update is called once per frame
    private void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var transform1 = transform;
        var move = transform1.right * x + transform1.forward * z;
        
        controller.Move(move * (speed * Time.deltaTime));
    }
}
