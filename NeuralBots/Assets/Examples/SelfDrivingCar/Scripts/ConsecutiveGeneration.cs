using System.Collections;
using UnityEngine;

namespace Evolutionary_perceptron.Examples.SelfDrivingCar
{
    using Evolutionary_perceptron.MendelMachine;
    public class ConsecutiveGeneration : MendelMachine
    {
        [Header("Consecutive data")]
        public Transform startPoint;
        public float lifeTime;

        [Header("Indicators")]
        [SerializeField] int index;

        protected override void Start()
        {
            base.Start();
            index = 0;
            StartCoroutine(InstantiateBotCoroutine(0.3f));
        }

        public override void NeuralBotDestroyed(NeuralBot neuralBot)
        {
            base.NeuralBotDestroyed(neuralBot);

            Destroy(neuralBot.gameObject);

            index++;

            if (index < individualsPerGeneration)
            {
                StartCoroutine(InstantiateBotCoroutine(0.3f));
            }
            else
            {
                index = 0; generation++;

                Save();
                population = Mendelization();
                StartCoroutine(InstantiateBotCoroutine(1));
            }
        }

        IEnumerator InstantiateBotCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            InstantiateBot(population[index], lifeTime, startPoint, index);
        }
    }
}
