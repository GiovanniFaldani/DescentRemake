using JetBrains.Annotations;       
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AttackController))]
public class Player : MonoBehaviour, IDamageable
{
    // tweakable parameters
    [Range(0, 6)] public int health = 6;
    [Range(0,14)] public int shield = 0;
    
    // normal mode movement
    [Tooltip("Speed the player moves at by default in normal mode")]
    [SerializeField] private float baseSpeed; // speed the player moves at by default in normal mode
    [Tooltip("Speed added or subtracted on the player's X axis with shift or ctrl each frame in normal mode")]
    [SerializeField] private float gearSpeed; // speed added or subtracted on the player's X axis with shift or ctrl each frame in normal mode
    [Tooltip("Maximum movement speed in both normal and hover mode")]
    [SerializeField] private float maxSpeed; // maximum movement speed in both normal and hover mode
    [Tooltip("Minimum movement speed in normal mode")]
    [SerializeField] private float minSpeed; // minimum movement speed in normal mode

    // hover mode movement
    [Tooltip("Speed added or subtracted on the player's X axis with W or S each frame in hover mode")]
    [SerializeField] private float xSpeed; // speed added or subtracted on the player's X axis with W or S each frame in hover mode
    [Tooltip("Speed added or subtracted on the player's Y axis with shift or ctrl each frame in hover mode")]
    [SerializeField] private float ySpeed; // speed added or subtracted on the player's Y axis with shift or ctrl each frame in hover mode
    [Tooltip("Speed added or subtracted on the player's Z axis with A or D each frame in hover mode")]
    [SerializeField] private float zSpeed; // speed added or subtracted on the player's Z axis with A or D each frame in hover mode
    [Tooltip("Speed used for rotations on the X axis (barrel rolling)")]
    [SerializeField] private float xAngleSpeed; // speed used for rotations on the X axis (barrel rolling)
    [Tooltip("Speed used for rotations on the Z axis (looking up and down)")]
    [SerializeField] private float zAngleSpeed; // speed used for rotations on the Z axis (looking up and down)

    private float yAngleSpeed; // speed used for rotations on the Y axis (turning left and right) - unused

    // bound variables
    [SerializeField] private int maxShield = 14;
    [SerializeField] private int maxHealth = 6;
    [SerializeField] private int maxEnergy = 10;

    // private internals
    private float baseMaxSpeed; // unused so far
    private float attackWait = 0;

    // Attacks stats
    [Tooltip("Energy available to the player")]
    [Range(1,10)] public int energyNumber; // public for use in UI
    [Tooltip("Time between attacks, modifies fire rate")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int fireBallCost = 1;
    [SerializeField] private int arcaneMissileCost = 2;
    [SerializeField] private int mineCost = 1;

    // component shorthands
    private Rigidbody rb;
    private AttackController ac;
    [SerializeField] private Transform attackSocketFront;
    [SerializeField] private Transform attackSocketBack;

    // Input
    private float gear, lr, ud, roll; // gear -> shift/ctrl input, lr -> A/D input, ud -> W/S input, roll -> unused
    private bool hovering; // hovering state, modifies all inputs
    private float fire1, fire2, fire3, fire4; // attack buttons

    // Camera
    //This is Main Camera in the Scene
    private Camera m_MainCamera;
    //This is the second Camera and is assigned in inspector
    [SerializeField] private Camera m_CameraTwo;
    // crosshair inserted via inspector
    [SerializeField] private RawImage crosshair;

    private void Awake()
    {
        // set internals
        baseMaxSpeed = maxSpeed;

        // get components
        rb = GetComponent<Rigidbody>();
        ac = GetComponent<AttackController>();
    }

    private void Start()
    {
        //This gets the Main Camera from the Scene
        m_MainCamera = Camera.main;
        //This enables Main Camera
        m_MainCamera.enabled = true;
        //Use this to disable secondary Camera
        m_CameraTwo.enabled = false;
    }

    private void Update()
    {
        HandleInputs();
        ApplyAttacks();
        ChangeView();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleInputs()
    {
        //movement
        gear = Input.GetAxisRaw("Gears");
        lr = Input.GetAxisRaw("Horizontal");
        ud = Input.GetAxisRaw("Vertical");
        //roll = Input.GetAxisRaw("xRoll"); // deprecated, not used anymore
        //hovering = System.Convert.ToBoolean(Input.GetAxisRaw("Jump"));
        hovering = Input.GetKey(KeyCode.Space);

        //attacks
        fire1 = Input.GetAxis("Fire1");
        fire2 = Input.GetAxis("Fire2");
        fire3 = Input.GetAxis("Fire3");
        fire4 = Input.GetAxis("Fire4");
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
                        //transform.up +  // no rotation on Y axis in this mode
                        transform.forward * -zAngleSpeed * ud   //W rotates the view down, S rotates the view up
                    )
                    * Time.fixedDeltaTime;
            rb.AddTorque(deltaRotation);
        }
    }

    private void ApplyAttacks()
    {
        // attack cooldown
        if (attackWait > 0)
        {
            attackWait -= Time.deltaTime;
        }
        else
        {
            if (fire1 > 0) // breath attack
            {
                ac.BreatheFire(this.transform.right, attackSocketFront, this.transform.rotation);
                attackWait = attackCooldown;
            }
            else if (fire2 > 0 && energyNumber > 0) // fireball attack
            {
                ac.ShootFireBall(this.transform.right, attackSocketFront);
                attackWait = attackCooldown;
                energyNumber -= fireBallCost;
            }
            else if (fire3 > 0 && energyNumber > 0) // arcane missile attack
            {
                ac.ShootArcaneMissile(this.transform.right, attackSocketFront);
                attackWait = attackCooldown;
                energyNumber -= arcaneMissileCost;
            }
            else if (fire4 > 0 && energyNumber > 0) // mine attack
            {
                ac.PlaceMine(attackSocketBack);
                attackWait = attackCooldown;
                energyNumber -= mineCost;
            }
        }
    }

    public void ChangeView()
    {
        if (Input.GetKey(KeyCode.R))
        {
            //Enable the second Camera
            m_CameraTwo.enabled = true;

            //The Main first Camera is disabled
            m_MainCamera.enabled = false;

            //Disable crosshair
            crosshair.enabled = false;
        }
        else
        {
            //Disable the second camera
            m_CameraTwo.enabled = false;

            //Enable the Main Camera
            m_MainCamera.enabled = true;

            // Enable crosshair 
            crosshair.enabled = true;
        }
        
    }

    void IDamageable.TakeDamage(int damage)
    {
        GameManager.Instance.playerUI.GetComponent<PlayerUI>().PrintToGameLog(
            "Took " + damage + " damage!", 5f);
        if (shield > 0)
        {
            shield -= damage;
            if (shield < 0)
            {
                shield = 0;
            }
        }
        else
        {
            this.health -= damage;
            if (health <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    public void AddShield(int shieldToAdd)
    {
        if (shield + shieldToAdd < maxShield)
            shield += shieldToAdd;
        else
            shield = maxShield;
    }

    public void AddEnergy(int energyToAdd)
    {
        if (energyNumber + energyToAdd < maxEnergy)
            energyNumber += energyToAdd;
        else
            energyNumber = maxEnergy;
    }
}
