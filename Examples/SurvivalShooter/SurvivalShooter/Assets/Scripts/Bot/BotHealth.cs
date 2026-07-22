using UnityEngine;

namespace EvolutionaryPerceptron.Examples.Survival
{
    public class BotHealth : Health
    {
        BotMovement playerMovement;                              // Reference to the player's movement.
        BotShooting playerShooting;                              // Reference to the PlayerShooting script.


        protected override void Awake ()
        {
            base.Awake();
            playerMovement = GetComponent <BotMovement> ();
            playerShooting = GetComponentInChildren <BotShooting> ();

        }

        public override void TakeDamage (int amount)
        {
            base.TakeDamage(amount);
            // Reduce the current health by the damage amount.

            SendMessage("ModifyFitness", -1);

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if(currentHealth <= 0 && !isDead)
            {
                // ... it should die.
                Death ();
            }
        }


        protected override void Death ()
        {
            base.Death();
            // Turn off any remaining shooting effects.
            playerShooting.DisableEffects ();


            // Turn off the movement and shooting scripts.
            playerMovement.enabled = false;
            playerShooting.enabled = false;
        }


    }
}