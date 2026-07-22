using UnityEngine;
using EvolutionaryPerceptron.MendelMachine;

namespace EvolutionaryPerceptron
{
    [RequireComponent(typeof(Brain))]
    public class BotHandler : MonoBehaviour
    {
        protected Brain nb;
        protected virtual void Start()
        {
            nb = GetComponent<Brain>();
        }

    }
}