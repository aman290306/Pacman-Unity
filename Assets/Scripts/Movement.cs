using UnityEngine;
//Generic movement script for Pacman and Ghosts
[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{ 
   public float speedMultiplier = 1.0f; // Speed multiplier

   public float speed = 8.0f; // Base speed

   public Vector2 initalDirection; // Initial movement direction

   public LayerMask obstacleLayer; // Layer for obstacles (choose which layers count as obstacles)

   public Rigidbody2D rigidBody2D { get; private set; }

   public Vector2 direction { get; private set; } // Current movement direction

   public Vector2 nextDirection { get; private set; } /* Next movement direction (if there is a wall above and you press up you can't go up until you
                                                        reach the spot where pacman aligns perfectly with the road leading up) */

   public Vector3 startingPosition { get; private set; } // Starting position of the GameObject

   private void Awake()
   {

    this.rigidBody2D = GetComponent<Rigidbody2D>();

    this.startingPosition = this.transform.position;

   }

    private void Start()
    {
         ResetState();
    }   

    public void ResetState()
    {
        this.speedMultiplier = 1.0f;
        this.direction = this.initalDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        this.rigidBody2D.isKinematic = false; // Make sure physics affects the object (example When ghosts leave home they become non-kinematic)
        this.enabled = true; // Enable this script, Sometimes we may want to disable movement (example when pacman dies)
    }

    private void Update()// Unity method called once per frame 
    {
        if (this.nextDirection != Vector2.zero)// If there is a next direction queued
        {
            SetDirection(this.nextDirection);
        }
    }

    public void FixedUpdate() // function is called at a fixed interval independent of frame rate unlike Update() which is called once per frame
    {
        Vector2 position = this.rigidBody2D.position; // Current position
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime; // Movement translation
        this.rigidBody2D.MovePosition(position + translation); // Move to new position
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (!Occupied(direction) || forced)// If the direction is not blocked or if forced is true
        {
            this.direction = direction;// Set current direction to the new direction
            this.nextDirection = Vector2.zero;// Clear next direction
        }
        else
        {
            this.nextDirection = direction;//
        }
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.rigidBody2D.position, Vector2.one * 0.75f, 0.0f, direction, 1.0f, this.obstacleLayer);// Cast a box in the given direction to check for obstacles
        return hit.collider != null;
    }
    
}

