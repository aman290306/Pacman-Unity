using UnityEngine;

public class PowerPellet : Pellet
{
   public float duration = 8f; // Duration of the power-up effect
   
   protected override void Eat()
   {
    FindObjectOfType<GameManager>().PowerPelletEaten(this);
   }
}
