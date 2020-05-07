using System.Collections.Generic;
using System.Linq;
using EvolutionaryPerceptron;
using UnityEngine;

namespace EvolutionaryPerceptron.MendelMachine {
    public class MendelMachine : MonoBehaviour {

        public enum PerformanceMethod {
            Mean,
            Median
        }

        [Header ("Data storage")]
        public DataManagement dataManagment;
        public bool useRelativeDataPath = true;
        public string dataPath;
        public int numberOfGenerationsToSave;
        public bool force;

        [Header ("Neural networks data")]
        public ActivationFunction activationFunction;
        public int[] neuronsPerLayer;
        public int seed;

        [Header ("Population parameters")]
        public bool sexualMultiplication;
        public bool exponentialFitness;
        public float mutationRate = 0.1f;
        public float maxPerturbation = 0.3f;
        public int elitism = 4;
        public int newIndividuals = 3;
        public int individualsPerGeneration = 10;
        public Brain prefab;

        [Header ("Indicators")]
        public PerformanceMethod performanceMethod;
        public int generation;
        public List<float> maxFitness = new List<float> ();
        public List<float> averageFitness = new List<float> ();
        [Header ("Custom")]
        public bool debug = true;
        public bool learningPhase = true;
        protected Individual[] population;
        protected System.Random r;

        protected virtual void Start () {
            generation = 0;
            r = new System.Random (seed);
            population = Load ();
            if (population == null) {
                GeneratePopulation ();
            }
        }
        protected virtual void GeneratePopulation () {
            if (debug)
                Debug.Log ("Population generated");

            population = new Individual[individualsPerGeneration];

            for (int i = 0; i < individualsPerGeneration; i++) {
                population[i] = GenerateIndividual ();
            }
        }

        protected virtual Brain InstantiateBot (
            Individual individual,
            float lifeTime,
            Transform placeToInstantiate,
            int index) {
            Brain nb = Instantiate (prefab,
                placeToInstantiate.position,
                placeToInstantiate.rotation);
            nb.Initialize (this, individual.gen, activationFunction, learningPhase, lifeTime, index);

            return nb;
        }

        protected virtual void Save () {
            if (force) {
                Handler.Save (population, dataPath, useRelativeDataPath, debug);
                return;
            }

            if (dataManagment != DataManagement.Save && dataManagment != DataManagement.SaveAndLoad) {
                if (debug)
                    Debug.Log (@"Data not saved because Data Manager is not Save or Save and load");
                return;
            }
            if (numberOfGenerationsToSave == 0) {
                if (debug)
                    Debug.Log (@"Number of generations to save can't be zero, 
                        if you don't want to save let data manager in Load or Nothing,
                        if you want to save in every generation let number of generations to save in 1");
                return;
            }

            if (generation % numberOfGenerationsToSave != 0)
                return;

            Handler.Save (population, dataPath, useRelativeDataPath, debug);

        }

        protected virtual Individual[] Load () {
            if (dataManagment != DataManagement.Load && dataManagment != DataManagement.SaveAndLoad) {
                if (debug)
                    Debug.Log (@"Data not loaded because Data Manager is not Load or Save and load");
                return null;
            }

            return Handler.Load (dataPath, useRelativeDataPath, debug);

        }

        protected virtual Individual[] Mendelization () {
            population = SortPopulation ();
            if (debug) {
                var m = population[0].fitness;
                var c = "Max fitness of generation " + generation + ",is: " + m;
                var avg = 0f;

                for (int i = 1; i < population.Length; i++) {
                    avg += population[i].fitness;
                    c += "\nIndividual :" + (i + 1) + " " + population[i].fitness.ToString ();
                }

                if (performanceMethod == PerformanceMethod.Mean) {
                    avg /= population.Length;
                    c = "Fitness average is :" + (avg).ToString () + "\n" + c;
                } else {
                    avg = population[(int) (population.Length / 2)].fitness;
                    c = "Fitness Median is :" + (avg).ToString () + "\n" + c;
                }

                Debug.Log (c);
                maxFitness.Add (m);
                averageFitness.Add (avg);
            }

            NormalizeFitness ();

            population = CrossPopulation ();
            population = MutatePopulation ();
            return population;
        }

        protected void NormalizeFitness () {
            if (exponentialFitness) {
                for (int i = 0; i < population.Count (); i++) {
                    population[i].fitness = Mathf.Pow (population[i].fitness, 2);
                }
            }

            var mean = 0.0f;
            for (int i = 0; i < population.Count (); i++) {
                mean += population[i].fitness;
            }

            mean /= population.Count ();

            for (int i = 0; i < population.Count (); i++) {
                population[i].fitness /= mean;
            }
        }

        public virtual void NeuralBotDestroyed (Brain neuralBot) {
            population[neuralBot.Index].fitness = neuralBot.Fitness;
        }

        protected Individual GenerateIndividual () {
            Individual i = new Individual {
                fitness = 0,
                gen = (new Perceptron (r, neuronsPerLayer, activationFunction)).GetGenoma
            };
            return i;
        }

        private Individual[] SortPopulation () {
            return population.OrderByDescending (o => o.fitness).ToArray ();
            //return SortedList.ToArray ();
        }

        private Individual[] CrossPopulation () {
            var tempPopulation = (Individual[]) population.Clone ();
            int populationSize = tempPopulation.Length;

            List<Individual> newpop = new List<Individual> ();

            for (int i = 0; i < elitism; i++) {
                newpop.Add (tempPopulation[i]);
            }
            for (int i = 0; i < newIndividuals; i++) {
                newpop.Add (GenerateIndividual ());
            }
            if (sexualMultiplication) {
                for (int i = 0; i < populationSize - newIndividuals - elitism; i++) {
                    var p1 = poolSelection (tempPopulation);
                    var p2 = poolSelection (tempPopulation);
                    Individual individual = new Individual {
                        gen = Genoma.Cross (
                        r,
                        p1.gen,
                        p2.gen
                        ),
                        fitness = 0
                    };

                    newpop.Add (individual);
                }
            } else {
                for (int i = 0; i < populationSize - newIndividuals - elitism; i++) {
                    var p1 = poolSelection (tempPopulation);
                    Individual individual = new Individual () {
                        gen = new Genoma (p1.gen.W),
                        fitness = 0
                    };
                    newpop.Add (individual);
                }
            }

            return newpop.ToArray ();
        }

        private Individual poolSelection (Individual[] pool) {
            var index = 0;
            var r = Random.Range (0f, 1f);
            while (r > 0) {
                r -= pool[index].fitness;
                index += 1;
            }
            index -= 1;
            return pool[index];
        }

        private Individual[] MutatePopulation () {
            int populationSize = population.Length;
            for (int i = elitism; i < populationSize; i++) {
                population[i].gen = Genoma.Mutate (
                    r,
                    population[i].gen,
                    mutationRate,
                    maxPerturbation
                );
            }
            return population;
        }
    }
}