using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Evolutionary_perceptron;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace Evolutionary_perceptron.MendelMachine
{
    using Evolutionary_perceptron.NeuralBot;

    [System.Serializable]
    public struct Individual
    {
        public Genoma gen;
        public float fitness;
    }

    public class Handler
    {
        public static Individual[] Load(string dataPath, bool useRelativeDataPath, bool debug)
        {

            Individual[] pop;

            if (dataPath == "" || dataPath == null)
                dataPath = useRelativeDataPath ? Application.dataPath + "/Data/data.bin" : "c://data.bin";
            else
                dataPath = (useRelativeDataPath ? Application.dataPath + "/Data" : "") + dataPath;

            if (!File.Exists(dataPath))
                return null;

            using (FileStream fs = new FileStream(dataPath, FileMode.Open))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    pop = (Individual[])formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    if (debug)
                        Debug.Log("Failed to deserialize. Reason: " + e.Message);
                    return null;
                }
                finally
                {
                    fs.Close();
                    if (debug)
                        Debug.Log("Data loaded");
                }
            }             

            return pop;

        }

        public static void Save(Individual[] ind, string dataPath, bool useRelativeDataPath, bool debug)
        {
            if (dataPath == "" || dataPath == null)
                dataPath = useRelativeDataPath ? Application.dataPath + "/Data/data.bin" : "c://data.bin";
            else
                dataPath = (useRelativeDataPath ? Application.dataPath + "/Data" : "") + dataPath;


            FileStream fs = new FileStream(dataPath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, ind);
            }
            catch (SerializationException e)
            {
                if (debug)
                    Debug.Log("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                if (debug)
                    Debug.Log("Data saved");
            }            
        }
    }

    public enum DataManagement
    {
        Save,
        Load,
        SaveAndLoad,
        Nothing
    }

    public class MendelMachine : MonoBehaviour
    {
        [Header("Data storage")]
        public DataManagement dataManagment;
        public bool useRelativeDataPath = true;
        public string dataPath;
        public int numberOfGenerationsToSave;

        [Header("Neural networks data")]
        public int[] neuronsPerLayer;
        public int seed;

        [Header("Population parameters")]
        public float mutationRate = 0.1f;
        public float maxPerturbation = 0.3f;
        public int elitism = 4;
        public int newIndividuals = 3;
        public int individualsPerGeneration = 10;
        public NeuralBot prefab;

        [Header("Indicators")]
        public int generation;
        public float maxFitness;
        public bool debug = true;
        public bool learningPhase = true;

        protected Individual[] population;
        protected System.Random r;
        
        

        protected virtual void Start()
        {
            generation = 0;
            maxFitness = -1;

            r = new System.Random(seed);

            population = Load();

            if (population == null)
            {
                GeneratePopulation();
            }
        }
        protected virtual void GeneratePopulation()
        {
            if (debug)
                Debug.Log("Population generated");

            population = new Individual[individualsPerGeneration];

            for (int i = 0; i < individualsPerGeneration; i++)
            {
                population[i] = GenerateIndividual();
            }
        }

        protected virtual NeuralBot InstantiateBot(Individual individual, float lifeTime, Transform placeToInstantiate, int index)
        {
            NeuralBot nb = Instantiate(prefab,
                    placeToInstantiate.position,
                    placeToInstantiate.rotation);
            //UnityEditor.Selection.activeGameObject = nb.gameObject;
            nb.Initialize(this, individual.gen, learningPhase, lifeTime, index);

            return nb;
        }

        protected virtual void Save()
        {
            if (dataManagment != DataManagement.Save && dataManagment != DataManagement.SaveAndLoad)
            {
                if (debug)
                    Debug.Log(@"Data not saved because Data Manager is not Save or Save and load");
                return;
            }
            if (numberOfGenerationsToSave == 0)
            {
                if (debug)
                    Debug.Log(@"Number of generations to save can't be zero, 
                        if you don't want to save let data manager in Load or Nothing,
                        if you want to save in every generation let number of generations to save in 1");
                return;
            }

            if (generation % numberOfGenerationsToSave != 0)
                return;

            Handler.Save(population, dataPath, useRelativeDataPath, debug);

        }

        protected virtual Individual[] Load()
        {            
            if (dataManagment != DataManagement.Load && dataManagment != DataManagement.SaveAndLoad)
            {
                if (debug)
                    Debug.Log(@"Data not loaded because Data Manager is not Load or Save and load");
                return null;
            }

            return Handler.Load(dataPath, useRelativeDataPath, debug);
            
        }

        protected virtual Individual[] Mendelization()
        {
            population = SortPopulation();

            maxFitness = population[0].fitness;

            if (debug)
                Debug.Log("Max fitness of generation " + generation +",is: " + maxFitness);

            population = CrossPopulation();
            population = MutatePopulation();
            return population;
        }

        public virtual void NeuralBotDestroyed(NeuralBot neuralBot)
        {
            population[neuralBot.index].fitness = neuralBot.fitness;
        }

        protected Individual GenerateIndividual()
        {
            Individual i = new Individual();
            i.fitness = 0;
            i.gen = (new Perceptron(r, neuronsPerLayer)).genoma;
            return i;
        }


        private Individual[] SortPopulation()
        {
            int populationSize = population.Length;
            bool sw = true;
            while (sw)
            {
                sw = false;
                for (int i = 1; i < populationSize; i++)
                {
                    if (population[i].fitness > population[i - 1].fitness)
                    {
                        Individual ph = population[i];
                        population[i] = population[i - 1];
                        population[i - 1] = ph;
                        sw = true;
                    }
                }
            }
            return population;
        }

        private Individual[] CrossPopulation()
        {
            int populationSize = population.Length;

            List<Individual> crosspop = new List<Individual>();

            for (int i = 0; i < populationSize; i++) 
            {
                population[i].fitness = -1;
                for (int j = 0; j < populationSize - i; j++)
                {
                    crosspop.Add(population[i]);
                }
            }

            List<Individual> newpop = new List<Individual>();

            for (int i = 0; i < elitism; i++)
            {
                newpop.Add(population[i]);
            }
            for (int i = 0; i < newIndividuals; i++)
            {
                newpop.Add(GenerateIndividual());
            }
            for (int i = 0; i < populationSize - newIndividuals - elitism; i++)
            {
                Individual ind = new Individual();
                ind.gen = Genoma.Cross(r,
                    crosspop[Random.Range(0, crosspop.Count)].gen,
                    crosspop[Random.Range(0, crosspop.Count)].gen);
                ind.fitness = 0;
                 
                newpop.Add(ind);
            }
            return newpop.ToArray();
        }

        private Individual[] MutatePopulation()
        {
            int populationSize = population.Length;
            for (int i = elitism; i < populationSize; i++)
            {
                population[i].gen = Genoma.Mutate(r, population[i].gen, mutationRate, maxPerturbation);
            }
            return population;
        }

        

    }

}

