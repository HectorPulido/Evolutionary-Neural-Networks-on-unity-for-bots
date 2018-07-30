using UnityEngine;

namespace EvolutionaryPerceptron.Examples.FlappyBird
{
    [RequireComponent(typeof(Flappy))]
    public class NeuralFlappy : BotHandler
    {
        Flappy flappy;

        protected override void Start()
        {
            base.Start();
            flappy = GetComponent<Flappy>();
        }

        void Update()
        {
            var output = nb.SetInput(GetInputs());

            if (output[0, 0] > 0.5f)
            {
                flappy.jumpRequest = true;
            }

            nb.AddFitness(Time.deltaTime);
        }

        float[,] GetInputs()
        {
            Obstacles o = null;

            var ray = Physics2D.Raycast(transform.position, Vector2.right, 999999);            
            var n1 = ray.collider != null ? ray.distance : 999999;
            if (ray.collider != null)
            {
                if (ray.collider.CompareTag("Obstacle"))
                    o = ray.collider.GetComponentInParent<Obstacles>();
            }
            ray = Physics2D.Raycast(transform.position, (Vector2.right + Vector2.down).normalized, 999999);
            var n2 = ray.collider != null ? ray.distance : 999999;

            if (o == null)
            {
                if (ray.collider != null)
                {
                    if (ray.collider.CompareTag("Obstacle"))
                        o = ray.collider.GetComponentInParent<Obstacles>();
                }
            }

            ray = Physics2D.Raycast(transform.position, (Vector2.right + Vector2.up).normalized, 999999);
            var n3 = ray.collider != null ? ray.distance : 999999;

            if (o == null)
            {
                if (ray.collider != null)
                {
                    if (ray.collider.CompareTag("Obstacle"))
                        o = ray.collider.GetComponentInParent<Obstacles>();
                }
            }

            var n4 = transform.position.y;

            var n5 = o == null ? 0 : o.center.position.y;

            return new float[1, 5] { { n1, n2, n3, n4, n5 } };
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Obstacle"))
            {
                nb.Destroy();
            }
        }

    }

}
