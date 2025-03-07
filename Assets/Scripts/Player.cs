using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // tweakable parameters
    [SerializeField] private int health = 6;
    [SerializeField][Range(0,14)] private int shield = 0;
    [SerializeField] private float xSpeed; // speed added on the player's X axis moving forward
    [SerializeField] private float maxSpeed; // maximum forward movement speed
    [SerializeField] private float xAngleSpeed; // speed used for rotations on the X axis (barrel rolling)
    [SerializeField] private float zAngleSpeed; // speed used for rotations on the Z axis (looking up and down)
    [SerializeField] private float yAngleSpeed; // speed used for rotations on the Y axis (turning left and right) - not really used

    // component shorthands
    private Rigidbody rb;

    // Input
    private float x, lr, ud, roll; // x -> forward movement, lr -> left/right rotation, ud -> up/down rotation, roll -> rotation on the X axis
    private bool hovering; // hovering state, modifies all inputs

    private void Awake()
    {
        // get components
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        HandleInputs();
       
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleInputs()
    {
        x = Input.GetAxisRaw("Vertical");
        lr = Input.GetAxisRaw("Horizontal");
        ud = Input.GetAxisRaw("UpDown");
        roll = Input.GetAxisRaw("xRoll");
    }

    private void ApplyMovement()
    {
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (Mathf.Abs(x) > 0 && rb.linearVelocity.magnitude > maxSpeed) x = 0;

        // apply forward and backward movement
        rb.AddForce(transform.right * x * xSpeed * Time.deltaTime);

        // apply rotation transforms (TODO: implement gradual snap to multiples of 90 on roll, but possibly not depending on GDD)
        Vector3 deltaRotation = (transform.right * xAngleSpeed * roll + transform.up * yAngleSpeed * lr + transform.forward * zAngleSpeed * ud) * Time.fixedDeltaTime;
        rb.AddTorque(deltaRotation);

    }
}
