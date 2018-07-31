using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
	using EvolutionaryPerceptron.MendelMachine;
	public class ArkanoidMendelMachine : MendelMachine {

		[Header("PONG Stuff")]
		public PongManager[] pongManagers;
		public float lifeTime;
		
		int index = 0;
		protected override void Start()
		{
			individualsPerGeneration = pongManagers.Length;
			base.Start(); 
			StartCoroutine(InstantiateBotCoroutine());
		}	
		//When a bot die
		public override void NeuralBotDestroyed(Brain neuralBot)
		{
			var b = neuralBot.GetComponent<NeuralRacket>().currentPongManager;
			var fitness = b.score2 - b.score1;
			neuralBot.AddFitness(fitness);
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
		//You can instantiate one, two, what you want
		IEnumerator InstantiateBotCoroutine()
		{
			//Instantiate bots
			yield return null;
			index = individualsPerGeneration;
			for	(int i = 0 ; i < individualsPerGeneration ; i++)
			{
				var b = InstantiateBot(population[i], lifeTime, pongManagers[i].initialRacket, i);
				b.transform.parent = pongManagers[i].transform;

				var n = b.GetComponent<NeuralRacket>();
				

				n.currentPongManager = pongManagers[i];
				pongManagers[i].currentRacket = n;
				pongManagers[i].Setup();
			}
		}
	}
}