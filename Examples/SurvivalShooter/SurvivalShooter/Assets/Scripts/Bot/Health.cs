using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Survival
{
    public class Health : MonoBehaviour
    {
        public int startingHealth = 100;                            // The amount of health the player starts the game with.
        public int currentHealth;                                   // The current health the player has.
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.

        Animator anim;                                              // Reference to the Animator component.
        protected bool isDead;                                                // Whether the player is dead.


        protected virtual void Awake()
        {
            // Setting up the references.
            anim = GetComponent<Animator>();

            // Set the initial health of the player.
            currentHealth = startingHealth;
        }

        public virtual void TakeDamage(int amount)
        {

            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (currentHealth <= 0 && !isDead)
            {
                // ... it should die.
                Death();
            }
        }


        protected virtual void Death()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;

            // Tell the animator that the player is dead.
            anim.SetTrigger("Die");
            SendMessage("Dead", SendMessageOptions.DontRequireReceiver);
        }


    }
}