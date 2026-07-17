using UnityEngine;

public class GhostScatter : GhostBehaviour
{   
    private void OnDisable()
    {
        this.ghost.chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();
        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
        int index = Random.Range(0, node.availableDirections.Count); // Choose a random available direction

        if(node.availableDirections[index] == -this.ghost.movement.direction && node.availableDirections.Count > 1)
        {
            index ++; // Avoid reversing direction
            if(index >= node.availableDirections.Count){
                index = 0;
            }
        }   
        this.ghost.movement.SetDirection(node.availableDirections[index]);
    }

   }
}
