using UnityEngine;

namespace EvolutionaryPerceptron.Examples.FlappyBird
{
    public class Flappy : MonoBehaviour
    {
        public float jumpSpeed;

        Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public bool jumpRequest;
        private void Update()
        {
            if (rb.velocity.y > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 45);
            }
            else if (rb.velocity.y < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, -45);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        private void FixedUpdate()
        {
            if (jumpRequest)
            {
                jumpRequest = false;
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }
        }
    }
}