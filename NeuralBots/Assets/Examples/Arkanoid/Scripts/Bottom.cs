using UnityEngine;
namespace Evolutionary_perceptron.Examples.Arkanoid
{
    public class Bottom : MonoBehaviour
    {
        Scenario scenario;

        void Start()
        {
            scenario = transform.parent.parent.GetComponent<Scenario>();
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Ball"))
            {
                ArkanoidMendelMachine.current.NeuralBotDestroyed(scenario.id);
            }
        }
    }
}
