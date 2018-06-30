using UnityEngine;
namespace Evolutionary_perceptron.Examples.SelfDrivingCar
{
    [RequireComponent(typeof(UnityStandardAssets.Vehicles.Car.CarController))]
    public class NeuralCar : BotHandler
    {
        UnityStandardAssets.Vehicles.Car.CarController m_Car;
        public Transform[] Sensors;
        public float maxDistance = 10f;

        protected override void Start()
        {
            base.Start();
            m_Car = GetComponent<UnityStandardAssets.Vehicles.Car.CarController>();
        }

        float[,] inputs;
        float[,] outputs;

        void Update()
        {
            inputs = SensorsInfo();
            outputs = nb.SetInput(inputs);
            m_Car.Move((float)outputs[0, 0],
                       1,
                       0,//(float)outputs[2] * 2f - 1,//(float)outputs[2] * 2f - 1,
                       0);//(float)outputs[3] * 2f - 1);
        }

        Ray r;
        float[,] SensorsInfo()
        {
            float[,] sensors = new float[1, Sensors.Length];
            for (int i = 0; i < Sensors.Length; i++)
            {
                r = new Ray(Sensors[i].position, Sensors[i].forward);

                Debug.DrawRay(r.origin, r.direction * maxDistance, Color.green, 0.01f);

                RaycastHit rh;
                Physics.Raycast(r, out rh, maxDistance);

                if (rh.collider == null)
                {
                    sensors[0, i] = maxDistance;
                }
                else
                {
                    sensors[0, i] = rh.distance;
                }

            }
            return sensors;
        }

        Collider lastFit;
        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Obstacle"))
            {
                nb.Destroy();
            }
            else if (col.CompareTag("Fit"))
            {
                if (col == lastFit)
                    return;
                lastFit = col;
                nb.AddFitness(1);
            }

        }
    }
}