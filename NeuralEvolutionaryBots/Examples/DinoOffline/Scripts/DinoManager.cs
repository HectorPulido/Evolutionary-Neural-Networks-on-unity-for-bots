using System.Collections;
using System.Collections.Generic;
using EvolutionaryPerceptron.MendelMachine;
using TMPro;
using UnityEngine;

namespace EvolutionaryPerceptron.Examples.Dino {
    public class DinoManager : EvolutionaryPerceptron.MendelMachine.MendelMachine {

        public static DinoManager singleton;

        [Header ("Manager properties")]
        public float scrollSpeed = 5;
        public float scrollAcceleration = 1;

        public float enemySpeed = 0.5f;
        public float enemyAcceleration = 0.2f;

        private float intialScrollSpeed = 5;
        private float intialEnemySpeed = 0.5f;

        public int index = 0; //Just one way to change the generation

        public float lifeTime = 20;
        public Transform initialPlace;

        public int generationFraction;

        int fraction = 0;

        protected override void Start () {
            if (singleton != null) {
                Destroy (this);
                return;
            }

            fraction = individualsPerGeneration;
            index = individualsPerGeneration;

            singleton = this;

            intialScrollSpeed = scrollSpeed;
            intialEnemySpeed = enemySpeed;

            base.Start ();
            StartCoroutine (InstantiateBotCoroutine ());
        }

        public void Lose () {
            //scrollSpeed = intialScrollSpeed;
            enemySpeed = intialEnemySpeed;

            var obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");

            foreach (GameObject obstacle in obstacles) {
                Destroy (obstacle);
            }
        }

        //When a bot die
        public override void NeuralBotDestroyed (Brain neuralBot) {
            base.NeuralBotDestroyed (neuralBot);

            //Doo some cool stuff, read the examples
            Destroy (neuralBot.gameObject); //Don't forget to destroy the gameObject

            index--;
            remaining--;

            if (index <= 0) {
                Save (); //don't forget to save when you change the generation
                population = Mendelization ();
                generation++;
                fraction = individualsPerGeneration;
                index = individualsPerGeneration;
                Lose ();
                StartCoroutine (InstantiateBotCoroutine ());
            } else {
                if (remaining == 0) {
                    Lose ();
                    StartCoroutine (InstantiateBotCoroutine ());
                }
            }
        }

        int remaining = 0;

        //You can instantiate one, two, what you want
        IEnumerator InstantiateBotCoroutine () {
            yield return new WaitForSeconds (0);
            //Instantiate bots
            for (int i = 0; i < generationFraction; i++) {
                fraction--;
                if (fraction < 0)
                    break;
                remaining++;
                var b = InstantiateBot (population[fraction], lifeTime, initialPlace, fraction);
            }
        }

    }
}