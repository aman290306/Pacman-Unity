using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehaviour
{
    public Transform inside; // Position where the ghost exits home
    public Transform outside; // Position where the ghost enters home

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        if(gameObject.activeInHierarchy)
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }
    
    private IEnumerator ExitTransition()
    {
        this.ghost.movement.SetDirection(Vector2.up, true);
        this.ghost.movement.rigidBody2D.isKinematic = true; // Make kinematic to avoid physics interference during transition
        this.ghost.movement.enabled = false; // Disable movement script during transition
        // animation
        Vector3 position = this.transform.position;
        float duration = 0.5f;
        float elapsed = 0.0f;
        while(elapsed < duration) // Move inside to outside over time
        {
            Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / duration);// Interpolate position over time. Lerp = Linear interpolation
            newPosition.z = position.z; // Keep original z position
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; 
        }

        elapsed = 0.0f;

        while(elapsed < duration) // Move outside to inside over time   
        {
            Vector3 newPosition = Vector3.Lerp(position, this.outside.position, elapsed / duration);// Interpolate position over time. Lerp = Linear interpolation
            newPosition.z = position.z; // Keep original z position
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0f),true); // Random left or right
        this.ghost.movement.rigidBody2D.isKinematic = false; // Re-enable physics
        this.ghost.movement.enabled = true; // Re-enable movement script
    }
}
