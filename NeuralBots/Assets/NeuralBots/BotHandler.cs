using UnityEngine;
using Evolutionary_perceptron.MendelMachine;

namespace Evolutionary_perceptron
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