using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace EvolutionaryPerceptron.MendelMachine
{
    public class Brain : MonoBehaviour
    {
        public int Index { get { return index; } }
        public float Fitness { get { return fitness; } }
        public bool On { get { return perceptron.W.Length > 0; } }
        float fitness;        
        int index;
        float lifeTime;
        MendelMachine mendelMachine;
        public Perceptron perceptron;
        public ActivationFunction activationFunction;
        public bool learningPhase;
        public string brainPath;


        public void AddFitness(float fitnessChange) { fitness += fitnessChange; }
        public double[,] SetInput(double[,] inputs)
        {
            if (On)
                return perceptron.ForwardPropagation(inputs);
            Debug.LogWarning("Brain is not ON");
            return null;
        }

        void Start()
        {
            if(!learningPhase && !string.IsNullOrEmpty(brainPath))
            {
                
                try
                    {
                        FileStream fs = new FileStream(brainPath, FileMode.Open);
                        BinaryFormatter formatter = new BinaryFormatter();
                        perceptron = new Perceptron(new Genoma((LinearAlgebra.Matrix[])formatter.Deserialize(fs)), activationFunction);
                        fs.Close();
                        Debug.Log("Perceptron loaded");
                    }
                    catch(SerializationException e)
                    {
                        Debug.LogError(e.Message);
                    }
            }

        }

        public void Initialize(MendelMachine mendelMachine, Genoma genoma, 
            ActivationFunction activationFunction, 
            bool learningPhase, float lifeTime, int index)
        {
            fitness = 0;

            this.activationFunction = activationFunction;
            this.index = index;
            this.mendelMachine = mendelMachine;
            this.learningPhase = learningPhase;
            this.lifeTime = lifeTime;

            perceptron = new Perceptron(genoma, activationFunction);
        }

        bool fail = false;
        public void Destroy ()
        {
            if (!learningPhase)
                return;
            if(!fail)
                mendelMachine.NeuralBotDestroyed(this);
            fail = true;                       
        }
                 
        protected void Update()
        {
            CheckLifetime();      
        }

        private void CheckLifetime()
        {
            if (!learningPhase)
                return;

            lifeTime -= Time.deltaTime;
            if (lifeTime < 0)
                Destroy();
        }
    }
}
