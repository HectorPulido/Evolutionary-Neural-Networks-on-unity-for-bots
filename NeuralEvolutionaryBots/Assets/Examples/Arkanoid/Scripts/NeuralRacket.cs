using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NeuralRacket : BotHandler
    {
        public float Velocity;

        Rigidbody2D rb;
        Transform ball;

        float[,] inputs;
        float[,] outputs;

        protected override void Start()
        {
            base.Start();

            ball = FindObjectOfType<Ball>().transform;
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            rb.gravityScale = 0;
        }

        void FixedUpdate()
        {
            nb.AddFitness(Time.fixedDeltaTime);

            Vector2 dis = transform.position - ball.position;

            inputs = SensorsInfo();
            inputs[0, sensors.Length] = dis.x;
            inputs[0, sensors.Length + 1] = dis.y;

            outputs = nb.SetInput(inputs);

            rb.velocity = new Vector3(Mathf.Clamp((float)outputs[0, 0], -Velocity, Velocity), 0);
        }

        public Transform[] sensors;
        public float maxDistance = 100f;
        public LayerMask mask;

        RaycastHit2D rh = new RaycastHit2D();
        float[,] SensorsInfo()
        {
            float[,] Sensors = new float[1, sensors.Length + 2];
            for (int i = 0; i < sensors.Length; i++)
            {
                rh = Physics2D.Raycast(sensors[i].position, sensors[i].up, maxDistance, mask);//(r, out rh, maxDistance);
                if (rh.collider == null)
                    Sensors[0, i] = maxDistance;
                else
                    Sensors[0, i] = rh.distance;

                Debug.DrawRay(sensors[i].position, sensors[i].up * (float)Sensors[0, i], Color.green, 0.01f);

            }
            return Sensors;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Ball"))
            {
                col.SendMessage("SetVelocity", new Vector2(rb.velocity.x, 5));
            }
        }

    }
}