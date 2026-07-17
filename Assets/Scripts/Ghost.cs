using UnityEngine;

public class Ghost : MonoBehaviour
{   
    public Movement movement { get; private set; } // Reference to the Movement component

    public GhostHome home { get; private set; } // Reference to the GhostHome behaviour
    public GhostScatter scatter { get; private set; } // Reference to the GhostScatter behaviour
    public GhostChase chase { get; private set; } // Reference to the GhostChase behaviour
    public GhostFrightened frightened { get; private set; } // Reference to the Ghost
    public GhostBehaviour initialBehaviour;
    public Transform target; // Target Transform for the ghost to chase
    public int points = 200; // Points awarded when this ghost is eaten
    
    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }
    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();
        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();
        
        if(this.home != this.initialBehaviour){ // Disable home if it is not the initial behaviour (Red ghost has home disabled at start)
            this.home.Disable();
        }
        
        if(this.initialBehaviour != null){
            this.initialBehaviour.Enable();
        } 
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))   
        {
            // Handle collision with Pacman
            if(this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);// Notify GameManager that this ghost has been eaten
            }
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();// Notify GameManager that Pacman has been eaten
            }
        }
    }
}
