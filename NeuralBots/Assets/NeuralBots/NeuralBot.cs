using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Evolutionary_perceptron.NeuralBot
{
    using MendelMachine;

    public enum PerceptronUpdate
    {
        Update,
        FixedUpdate,
        Manual
    }

    public class NeuralBot : MonoBehaviour
    {
        [Header("BotData")]
        int _index;
        public int index { get { return _index; } }
        [SerializeField]
        bool on;
        [SerializeField]
        float _fitness;
        public float fitness { get { return _fitness; } }
        [SerializeField]
        float _lifeTime;
        [SerializeField]
        bool _learningPhase;
        [SerializeField]
        MendelMachine _mendelMachine;
        [SerializeField]
        Perceptron _perceptron;


        public void AddFitness(float fitnessChange) { _fitness += fitnessChange; }
        public float[,] SetInput(float[,] inputs)
        {
            if (on)
                return _perceptron.ForwardPropagation(inputs);
            return null;
        }

        public void Initialize(MendelMachine mendelMachine, Genoma genoma, 
            bool learningPhase, float lifeTime, int index)
        {
            _index = index;
            _fitness = 0;
            _mendelMachine = mendelMachine;
            _perceptron = new Perceptron(genoma);
            _learningPhase = learningPhase;
            _lifeTime = lifeTime;
            on = true;
        }

        bool fail = false;

        public void Destroy ()
        {
            if (!_learningPhase)
                return;
            if(!fail)
                _mendelMachine.NeuralBotDestroyed(this);
            fail = true;                       
        }
                 
        protected void Update()
        {
            CheckLifetime();      
        }

        private void CheckLifetime()
        {
            if (!_learningPhase)
                return;

            _lifeTime -= Time.deltaTime;
            if (_lifeTime < 0)
                Destroy();
        }
    }
}
