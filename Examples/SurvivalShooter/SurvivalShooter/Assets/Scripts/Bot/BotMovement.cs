using UnityEngine;

namespace EvolutionaryPerceptron.Examples.Survival
{
    public class BotMovement : MonoBehaviour
    {
        public float speed = 6f;            // The speed that the player will move at.
        public float turnSpeed = 90f;

        Vector3 movement;                   // The vector to store the direction of the player's movement.
        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.

        void Awake ()
        {
            // Set up references.
            anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();
        }

        // Store the input axes.
        [HideInInspector]
        public float h ;
        [HideInInspector]
        public float v ;

        void FixedUpdate ()
        {
            // Move the player around the scene.
            Move ();

            // Turn the player to face the mouse cursor.
            Turning ();

            // Animate the player.
            Animating ();
        }


        void Move ()
        {
            // Set the movement vector based on the axis input.
            movement = transform.forward * v + transform.right*h;
                        
            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * speed;

            // Move the player to it's current position plus the movement.
            playerRigidbody.velocity = movement;
            //playerRigidbody.MovePosition (transform.position + movement);
        }

        [HideInInspector]
        public float turn;
        void Turning ()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * turn * turnSpeed);
        }


        void Animating ()
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            anim.SetBool ("IsWalking", walking);
        }
    }
}