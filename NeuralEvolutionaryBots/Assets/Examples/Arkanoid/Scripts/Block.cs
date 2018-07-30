using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
    public class Block : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Ball"))
            {
                if (col.GetComponent<Ball>().inv)
                {
                    Destroy(gameObject);
                }
                col.SendMessage("InverRbVelocityY");

            }
        }
    }
}