using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    private SpriteRenderer spriterenderer;
    private Movement movement;

    private void Awake() // function is called when the script instance is being loaded
    {
        this.spriterenderer = GetComponent<SpriteRenderer>();
        this.movement = GetComponentInParent<Movement>();
    }
    private void Update()
    {
        if(this.movement.direction == Vector2.up)
        {
            this.spriterenderer.sprite = up;
        }
        else if(this.movement.direction == Vector2.down)
        {
            this.spriterenderer.sprite = down;
        }
        else if(this.movement.direction == Vector2.left)
        {
            this.spriterenderer.sprite = left;
        }
        else if(this.movement.direction == Vector2.right)
        {
            this.spriterenderer.sprite = right;
        }
    }

}
