using UnityEngine;

public class GhostFrightened : GhostBehaviour
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;
    public bool eaten { get; private set; }
    
    private void Eaten()
    {
        this.eaten=true;

        Vector3 position = this.ghost.home.inside.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;
        // Send ghost back to home
        this.ghost.home.Enable(this.duration); // Enable home behaviour for the duration of frightened mode

        this.body.enabled = false;
        this.eyes.enabled = true;
        this.white.enabled = false;
        this.blue.enabled = false;
    }

    public override void Enable(float duration)
    {
        base.Enable(duration);
        this.body.enabled = false;
        this.eyes.enabled = false;  
        this.blue.enabled = true;
        this.white.enabled = false;
        Invoke(nameof(Flash), duration / 2.0f); // Start flashing halfway through frightened mode
    }
    public override void Disable()
    {
        base.Disable();
        this.body.enabled = true;
        this.eyes.enabled = true;  
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    private void Flash()
    {
        if(!this.eaten)
        {
            this.blue.enabled = false;
            this.white.enabled = true;
            this.white.GetComponent<AnimatedSprite>().ResetAnimation();
        }
    }
    private void OnEnable()
    {
        this.ghost.movement.speedMultiplier = 0.5f; // Reduce speed when frightened
        this.eaten = false;
        

    }
    private void OnDisable()
    {
        this.ghost.movement.speedMultiplier = 1.0f; // Restore speed
        this.eaten = false;
        
    }
     public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))   
        {
            // Handle collision with Pacman
            if(this.enabled)
            {
                Eaten();
            }
        }   
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();
        if( node != null && this.enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue; // Initialize to minimum possible value

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude; // Use sqrMagnitude for performance (no square root calculation)
            
                if (distance >= maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }
            this.ghost.movement.SetDirection(direction);
        }
    }
}
