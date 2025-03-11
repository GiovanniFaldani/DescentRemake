using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // tweakable parameters
    [SerializeField] private int health = 6;
    [SerializeField][Range(0,14)] private int shield = 0;
    
    // normal mode movement
    [Tooltip("speed the player moves at by default in normal mode")]
    [SerializeField] private float baseSpeed; // speed the player moves at by default in normal mode
    [Tooltip("speed added or subtracted on the player's X axis with shift or ctrl each frame in normal mode")]
    [SerializeField] private float gearSpeed; // speed added or subtracted on the player's X axis with shift or ctrl each frame in normal mode
    [Tooltip("maximum movement speed in both normal and hover mode")]
    [SerializeField] private float maxSpeed; // maximum movement speed in both normal and hover mode
    [Tooltip("minimum movement speed in normal mode")]
    [SerializeField] private float minSpeed; // minimum movement speed in normal mode

    // hover mode movement
    [Tooltip("speed added or subtracted on the player's X axis with W or S each frame in hover mode")]
    [SerializeField] private float xSpeed; // speed added or subtracted on the player's X axis with W or S each frame in hover mode
    [Tooltip("speed added or subtracted on the player's Y axis with shift or ctrl each frame in hover mode")]
    [SerializeField] private float ySpeed; // speed added or subtracted on the player's Y axis with shift or ctrl each frame in hover mode
    [Tooltip("speed added or subtracted on the player's Z axis with A or D each frame in hover mode")]
    [SerializeField] private float zSpeed; // speed added or subtracted on the player's Z axis with A or D each frame in hover mode
    [Tooltip("speed used for rotations on the X axis (barrel rolling)")]
    [SerializeField] private float xAngleSpeed; // speed used for rotations on the X axis (barrel rolling)
    [Tooltip("speed used for rotations on the Z axis (looking up and down)")]
    [SerializeField] private float zAngleSpeed; // speed used for rotations on the Z axis (looking up and down)

    private float yAngleSpeed; // speed used for rotations on the Y axis (turning left and right) - unused

    // private internals
    private float baseMaxSpeed;

    // component shorthands
    private Rigidbody rb;

    // Input
    private float gear, lr, ud, roll; // gear -> shift/ctrl input, lr -> A/D input, ud -> W/S input, roll -> unused
    private bool hovering; // hovering state, modifies all inputs

    private void Awake()
    {
        // set internals
        baseMaxSpeed = maxSpeed;

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
        gear = Input.GetAxisRaw("Gears");
        lr = Input.GetAxisRaw("Horizontal");
        ud = Input.GetAxisRaw("Vertical");
        //roll = Input.GetAxisRaw("xRoll"); // deprecated, not used anymore
        hovering = System.Convert.ToBoolean(Input.GetAxisRaw("Jump"));
    }

    private void ApplyMovement()
    {
        // mode controls
        if (hovering) // hovering mode
        {
            // If speed is larger than maxspeed, cancel out the input so you don't go over
            if (ud > 0 && rb.linearVelocity.magnitude > maxSpeed) ud = 0;
            if (gear > 0 && rb.linearVelocity.magnitude > maxSpeed) gear = 0;
            if (lr > 0 && rb.linearVelocity.magnitude > maxSpeed) lr = 0;

            // apply axis movement
            Vector3 deltaForce = (
                        transform.right * ud * xSpeed +
                        transform.up * gear * ySpeed + 
                        transform.forward * -lr * zSpeed // forward is at the player's left so the - is to get the right, idk why I'm using this system of reference
                    ) 
                    * Time.fixedDeltaTime;
            rb.AddForce(deltaForce);
        }
        else // normal mode
        {
            // If speed is larger than maxspeed or lower than minspeed, cancel out the input so you don't go over or under
            if (gear > 0 && rb.linearVelocity.magnitude > maxSpeed) gear = 0;
            if (gear < 0 && rb.linearVelocity.magnitude < minSpeed) gear = 0;

            // move at constant speed if no input
            if (Mathf.Abs(gear) < float.Epsilon && rb.linearVelocity.magnitude < baseSpeed)
            {
                rb.AddForce(transform.right * baseSpeed * Time.fixedDeltaTime);
            }

            // apply forward and backward movement
            rb.AddForce(transform.right * gear * gearSpeed * Time.fixedDeltaTime);

            // apply rotation
            Vector3 deltaRotation = (
                    transform.right * xAngleSpeed * -lr +  // A rolls the player left (counter clockwise), and D rolls the player right (clockwise)
                    transform.up +  // no rotation on Y axis in this mode
                    transform.forward * -zAngleSpeed * ud   //W rotates the view down, S rotates the view up
                )
                * Time.fixedDeltaTime;
            rb.AddTorque(deltaRotation);
        }

    }
}
