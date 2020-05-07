using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid {
    [RequireComponent (typeof (Rigidbody2D))]
    public class NeuralRacket : BotHandler {
        public float velocity;
        public PongManager currentPongManager;

        private Rigidbody2D rb;
        private Rigidbody2D ball;
        private Rigidbody2D enemyRacket;

        private double[, ] inputs;
        private double[, ] outputs;

        protected override void Start () {
            base.Start ();
            rb = GetComponent<Rigidbody2D> ();
            ball = currentPongManager.ballRb;
            enemyRacket = currentPongManager.racket;
            lastInputs = new double[1, 7 * 2];
        }

        private float v;
        private double[, ] lastInputs;
        private void Update () {
            var timeDiference = Time.deltaTime;
            inputs = new double[1, 7 * 2] {
                {
                ball.transform.localPosition.x,
                ball.transform.localPosition.y,
                ball.velocity.x,
                ball.velocity.y,
                enemyRacket.transform.localPosition.y,
                enemyRacket.transform.localPosition.y,
                transform.transform.localPosition.y,
                0, 0, 0, 0, 0, 0, 0
                }
            };

            for (int i = 0; i < 7; i++) {
                inputs[0, i + 7] = (inputs[0, i] - lastInputs[0, i]) * timeDiference;
            }

            lastInputs = (double[, ]) inputs.Clone ();

            outputs = nb.SetInput (inputs);
            v = (float) outputs[0, 0];
        }
        private void FixedUpdate () {
            var vel = v * velocity;
            vel = Mathf.Clamp (vel, -velocity, velocity);

            rb.velocity = new Vector2 (0, vel);
        }
        public void DestroySelf () {
            nb.Destroy ();
        }
    }
}