using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NeuralRacket : BotHandler
    {
        public float velocity;
        public PongManager currentPongManager;

        Rigidbody2D rb;
        Rigidbody2D ball;
        Rigidbody2D enemyRacket;
        

        double[,] inputs;
        double[,] outputs;

        protected override void Start()
        {
            base.Start();
            rb = GetComponent<Rigidbody2D>();
            ball = currentPongManager.ballRb;
            enemyRacket = currentPongManager.racket;
        }

        float v;
        void Update()
        {
            inputs = new double[,]{{ball.transform.localPosition.x, ball.transform.localPosition.y, 
                                    ball.velocity.x, ball.velocity.y, 
                                    enemyRacket.transform.localPosition.y ,enemyRacket.transform.localPosition.y, 
                                    transform.transform.localPosition.y}};

            outputs = nb.SetInput(inputs);
            v = (float)outputs[0,0];
        }
        void FixedUpdate()
        {
			var vel = v * velocity;
			vel = Mathf.Clamp(vel, -velocity, velocity);

			rb.velocity = new Vector2(0, vel);
        }
    }
}