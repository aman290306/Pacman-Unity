using UnityEngine;

public class Pellet : MonoBehaviour
{
   public int points = 10; // Points awarded when this pellet is eaten

   protected virtual void Eat(){
       FindObjectOfType<GameManager>().PelletEaten(this);
   }
   
   private void OnTriggerEnter2D(Collider2D other)
   {
       if(other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
       {
           Eat();
       }
   }
}
