using UnityEngine;
using System.Collections;

namespace Evolutionary_perceptron.Examples.FlappyBird
{
    using Evolutionary_perceptron.MendelMachine;

    public class FlappyMendelMachine : MendelMachine
    {
        [Header("FlappyStuff")]
        public Transform startPoint;
        public Obstacles[] obstacles;

        int index = 15;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(InstantiateBotCoroutine());
        }
        public override void NeuralBotDestroyed(NeuralBot neuralBot)
        {
            base.NeuralBotDestroyed(neuralBot);

            Destroy(neuralBot.gameObject);

            index--;

            if (index <= 0)
            {
                Save();
                population = Mendelization();
                generation++;

                StartCoroutine(InstantiateBotCoroutine());
            }
        }
        IEnumerator InstantiateBotCoroutine()
        {
            yield return null;
            index = individualsPerGeneration;

            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].ReturnToStart();
            }

            for (int i = 0; i < population.Length; i++)
            {
                InstantiateBot(population[i], 999999, startPoint, i);
            }
        }
    }
}