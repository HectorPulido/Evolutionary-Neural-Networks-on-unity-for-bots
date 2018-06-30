using System.Collections;
using UnityEngine;
namespace Evolutionary_perceptron.Examples.Arkanoid
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        public float startVelocity;
        public bool inv = true;
        Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0;

            Setup();
        }
        void Setup()
        {
            transform.localPosition = Vector2.zero;
            rb.velocity = new Vector2(Random.Range(-1, 2), -1) * startVelocity;
        }
        void SetVelocity(Vector2 vel)
        {
            rb.velocity = vel;
        }
        void InverRbVelocityY()
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            if (!inv)
                return;
            inv = false;
            StartCoroutine("invTrue");
        }
        IEnumerator InvTrue()
        {
            yield return new WaitForSeconds(0.1f);
            inv = true;
        }
    }
}