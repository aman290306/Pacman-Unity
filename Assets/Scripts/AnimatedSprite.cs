using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class AnimatedSprite : MonoBehaviour
{
  public SpriteRenderer spriteRenderer { get; private set; }

  public float animationTime=0.25f; // Time between frames

  public Sprite[] sprites; // Array of sprites for animation

  public int animationFrame { get; private set; } // Current frame index

  public bool loop = true; 

  private void Awake()
  {
    this.spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Start()
  {
    InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
  }

  private void Advance()
  {
    this.animationFrame++;
    
    if(this.animationFrame >= this.sprites.Length && this.loop){
        this.animationFrame=0;
    }
    if(this.animationFrame >=0 && this.animationFrame < this.sprites.Length){
        this.spriteRenderer.sprite=this.sprites[this.animationFrame];
    }
  }
  
  public void ResetAnimation()
  {
    this.animationFrame=-1;
    Advance();
  }

}
