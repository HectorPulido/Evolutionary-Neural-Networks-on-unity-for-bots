using UnityEngine;
using EvolutionaryPerceptron.MendelMachine;

namespace EvolutionaryPerceptron
{
    [RequireComponent(typeof(NeuralBot))]
    public class BotHandler : MonoBehaviour
    {
        protected NeuralBot nb;
        protected virtual void Start()
        {
            nb = GetComponent<NeuralBot>();
        }

    }
}