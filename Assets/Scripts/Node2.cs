using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;

    public readonly List<Vector2> availableDirections = new();
    public List<Node> neighbors = new(); // 🔥 NEW

    // A* variables
    [HideInInspector] public float gCost;
    [HideInInspector] public float hCost;
    [HideInInspector] public Node parent;

    public float fCost => gCost + hCost;

    private void Start()
    {
        availableDirections.Clear();
        neighbors.Clear();

        CheckDirection(Vector2.up);
        CheckDirection(Vector2.down);
        CheckDirection(Vector2.left);
        CheckDirection(Vector2.right);
    }

    private void CheckDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            this.transform.position,
            Vector2.one * 0.5f,
            0f,
            direction,
            1f,
            this.obstacleLayer
        );

        if (hit.collider == null)
        {
            availableDirections.Add(direction);

            RaycastHit2D nodeHit = Physics2D.Raycast(
                this.transform.position,
                direction,
                1f
            );

            if (nodeHit.collider != null)
            {
                Node neighbor = nodeHit.collider.GetComponent<Node>();
                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
                }
            }
        }
    }
}