using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Evolutionary_perceptron.NeuralBot;

[RequireComponent(typeof(UnityStandardAssets.Vehicles.Car.CarController))]
[RequireComponent(typeof(NeuralBot))]
public class NeuralCar : MonoBehaviour
{
    UnityStandardAssets.Vehicles.Car.CarController m_Car;
    NeuralBot neuralBot;
    public Transform[] Sensors;
    public float maxDistance = 10f;

    void Start()
    {
        neuralBot = GetComponent<NeuralBot>();
        m_Car = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
    }

    double[,] inputs;
    double[,] outputs;

    void Update ()
    {
        inputs = SensorsInfo();
        outputs = neuralBot.SetInput(inputs);
        m_Car.Move((float)outputs[0,0],
                   1,
                   0,//(float)outputs[2] * 2f - 1,//(float)outputs[2] * 2f - 1,
                   0);//(float)outputs[3] * 2f - 1);
    }

    Ray r;
    double[,] SensorsInfo()
    {
        double[,] sensors = new double[1 , Sensors.Length];
        for (int i = 0; i < Sensors.Length; i++)
        {
             r = new Ray(Sensors[i].position, Sensors[i].forward);

            Debug.DrawRay(r.origin, r.direction * maxDistance, Color.green, 0.01f);

            RaycastHit rh;
            Physics.Raycast(r, out rh, maxDistance);

            if (rh.collider == null)
            {
                sensors[0,i] = maxDistance;
            }
            else
            {
                sensors[0,i] = rh.distance ;
            }
            
        }
        return sensors;
    }

    Collider lastFit;
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Obstacle"))
        {
            neuralBot.Destroy();
        }
        else if (col.CompareTag("Fit"))
        {
            if (col == lastFit)
                return;
            lastFit = col;
            neuralBot.AddFitness(1);
        }
        
    }
}
