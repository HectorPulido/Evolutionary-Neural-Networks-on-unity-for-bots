using UnityEngine;

namespace EvolutionaryPerceptron.Examples.FlappyBird {
    [RequireComponent (typeof (Flappy))]
    public class NeuralFlappy : BotHandler {
        public bool showRays;
        public float maxDistance;
        private Flappy flappy;
        private int inputSize;
        private double[, ] lastInputs;
        private double[, ] inputs;

        protected override void Start () {
            base.Start ();
            flappy = GetComponent<Flappy> ();
            inputSize = 10;
            lastInputs = new double[1, inputSize];
        }

        private void Update () {
            var time = Time.deltaTime;
            inputs = GetInputs ();
            inputs = ProcessInputs (inputs, time);
            var output = nb.SetInput (inputs);
            if (output[0, 0] > 0.5f) {
                flappy.jumpRequest = true;
            }

            nb.AddFitness (time);
        }

        private double[, ] ProcessInputs (double[, ] inputs, double time) {
            var currentInput = new double[1, inputSize]; // Sensor info
            for (var i = 0; i < inputSize / 2; i++) {
                currentInput[0, i] = inputs[0, i];
            }

            for (var i = 0; i < inputSize / 2; i++) {
                currentInput[0, i + inputSize / 2] = (currentInput[0, i] - lastInputs[0, i]) * time;
            }

            lastInputs = (double[, ]) currentInput.Clone ();

            return currentInput;
        }

        private double[, ] GetInputs () {
            Obstacles o = null;

            var ray = Physics2D.Raycast (transform.position, Vector2.right, maxDistance);
            var n1 = ray.collider != null ? ray.distance : maxDistance;

            if (showRays) {
                Debug.DrawRay (transform.position, Vector2.right * n1);
            }

            if (ray.collider != null) {
                if (ray.collider.CompareTag ("Obstacle"))
                    o = ray.collider.GetComponentInParent<Obstacles> ();
            }
            ray = Physics2D.Raycast (transform.position, (Vector2.right + Vector2.down).normalized, maxDistance);
            var n2 = ray.collider != null ? ray.distance : maxDistance;

            if (showRays) {
                Debug.DrawRay (transform.position, (Vector2.right + Vector2.down).normalized * n2);
            }

            if (o == null) {
                if (ray.collider != null) {
                    if (ray.collider.CompareTag ("Obstacle"))
                        o = ray.collider.GetComponentInParent<Obstacles> ();
                }
            }

            ray = Physics2D.Raycast (transform.position, (Vector2.right + Vector2.up).normalized, maxDistance);
            var n3 = ray.collider != null ? ray.distance : maxDistance;

            if (showRays) {
                Debug.DrawRay (transform.position, (Vector2.right + Vector2.up).normalized * n3);
            }

            if (o == null) {
                if (ray.collider != null) {
                    if (ray.collider.CompareTag ("Obstacle"))
                        o = ray.collider.GetComponentInParent<Obstacles> ();
                }
            }

            var n4 = transform.position.y;

            var n5 = o == null ? 0 : o.center.position.y;

            return new double[1, 5] { { n1, n2, n3, n4, n5 } };
        }

        private void OnTriggerEnter2D (Collider2D collision) {
            if (collision.CompareTag ("Obstacle")) {
                nb.Destroy ();
            }
        }

    }

}