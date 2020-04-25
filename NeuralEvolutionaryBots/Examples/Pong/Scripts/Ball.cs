using System.Collections;
using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        public float startVelocity;
        Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0;
        }
        public void Setup()
        {
            transform.localPosition = Vector2.zero;
            rb.velocity = new Vector2(1, 1) * startVelocity;
        }
       
    }
}