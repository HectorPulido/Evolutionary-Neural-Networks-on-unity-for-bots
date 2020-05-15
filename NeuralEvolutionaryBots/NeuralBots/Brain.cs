using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace EvolutionaryPerceptron.MendelMachine {
    public class Brain : MonoBehaviour {
        public int Index { get { return index; } }
        public float Fitness { get { return fitness; } }
        public bool On { get { return perceptron.W.Length > 0; } }
        public float lifeTime;
        MendelMachine mendelMachine;
        public Perceptron perceptron;
        public ActivationFunction activationFunction;
        public bool learningPhase;
        [Header ("You can save the brain from the context menu")]
        public string brainPath;
        float fitness;
        int index;
        private bool fail = false;


        public void AddFitness (float fitnessChange) { fitness += fitnessChange; }
        public double[, ] SetInput (double[, ] inputs) {
            if (On)
                return perceptron.ForwardPropagation (inputs);
            Debug.LogWarning ("Brain is not ON");
            return null;
        }

        [ContextMenu ("Save brain")]
        private void Save () {
            if (!string.IsNullOrEmpty (brainPath)) {
                FileStream fs = new FileStream (brainPath, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter ();
                try {
                    formatter.Serialize (fs, perceptron.GetGenoma);
                } catch (SerializationException e) {
                    Debug.Log ("Failed to serialize. Reason: " + e.Message);
                    throw;
                } finally {
                    fs.Close ();
                    Debug.Log ("Data saved");
                }
            } else {
                Debug.Log ("Invalid path");
            }

        }

        private void Start () {
            if (!learningPhase && !string.IsNullOrEmpty (brainPath)) {

                try {
                    FileStream fs = new FileStream (brainPath, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter ();
                    perceptron = new Perceptron ((Genoma) formatter.Deserialize (fs), activationFunction);
                    fs.Close ();
                    Debug.Log ("Perceptron loaded");
                } catch (SerializationException e) {
                    Debug.LogError (e.Message);
                }
            }

        }

        public void Initialize (MendelMachine mendelMachine, Genoma genoma,
            ActivationFunction activationFunction,
            bool learningPhase, float lifeTime, int index) {
            fitness = 0;

            this.activationFunction = activationFunction;
            this.index = index;
            this.mendelMachine = mendelMachine;
            this.learningPhase = learningPhase;
            this.lifeTime = lifeTime;

            perceptron = new Perceptron (genoma, activationFunction);
        }

        public void Destroy () {
            if (!learningPhase)
                return;
            if (!fail)
                mendelMachine.NeuralBotDestroyed (this);
            fail = true;
        }

        protected void Update () {
            CheckLifetime ();
        }

        private void CheckLifetime () {
            if (!learningPhase)
                return;

            lifeTime -= Time.deltaTime;
            if (lifeTime < 0)
                Destroy ();
        }
    }
}