using UnityEngine;
namespace EvolutionaryPerceptron.MendelMachine
{
    public class NeuralBot : MonoBehaviour
    {
        [Header("BotData")]
        int index;
        public int Index { get { return index; } }
        [SerializeField]
        bool on;
        [SerializeField]
        float fitness;
        public float Fitness { get { return fitness; } }
        [SerializeField]
        float lifeTime;
        [SerializeField]
        bool learningPhase;
        [SerializeField]
        MendelMachine mendelMachine;
        [SerializeField]
        Perceptron perceptron;


        public void AddFitness(float fitnessChange) { fitness += fitnessChange; }
        public float[,] SetInput(float[,] inputs)
        {
            if (on)
                return perceptron.ForwardPropagation(inputs);
            return null;
        }

        public void Initialize(MendelMachine mendelMachine, Genoma genoma, 
            ActivationFunction activationFunction, 
            bool learningPhase, float lifeTime, int index)
        {
            fitness = 0;

            this.index = index;
            this.mendelMachine = mendelMachine;
            this.learningPhase = learningPhase;
            this.lifeTime = lifeTime;

            perceptron = new Perceptron(genoma, activationFunction);
            on = true;
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
