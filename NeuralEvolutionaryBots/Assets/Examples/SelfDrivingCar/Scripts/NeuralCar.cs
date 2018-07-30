using UnityEngine;
namespace EvolutionaryPerceptron.Examples.SelfDrivingCar
{
    using UnityStandardAssets.Vehicles.Car;

    [RequireComponent(typeof(CarController))]
    public class NeuralCar : BotHandler
    {
        CarController cc;
        Rigidbody rb;
        public Transform eyes;
        public int rayCount;
        public int angles;
        public float maxDistance = 10f;

        protected override void Start()
        {
            base.Start();
            cc = GetComponent<CarController>();
            rb = GetComponent<Rigidbody>();

            d = new float[1,rayCount + 2];
        }

        float[,] inputs;
        float[,] outputs;

        void Update()
        {
            inputs = SensorsInfo();
            outputs = nb.SetInput(inputs);

            cc.Move((float)outputs[0, 0],
                    (float)outputs[0, 1],
                    (float)outputs[0, 1],//(float)outputs[2] * 2f - 1,//(float)outputs[2] * 2f - 1,
                       0);//(float)outputs[3] * 2f - 1);
        }

        float[,] d;
        Ray r;
        RaycastHit rh;
        float angle;
        float[,] SensorsInfo()
        {
            for (int i = 0; i < rayCount; i++)
            {
                angle = angles * (i - rayCount / 2.0f) / rayCount;
                angle += transform.eulerAngles.y;
                angle *= Mathf.Deg2Rad;
                r = new Ray(eyes.position, new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)));
                Physics.Raycast(r, out rh, maxDistance);
                if (rh.collider != null)
                    d[0, i] = rh.distance / maxDistance;                
                else                
                    d[0, i] = 1;                

                Debug.DrawRay(r.origin, r.direction * d[0, i] * maxDistance, Color.green);
            }
            d[0, rayCount + 0] = cc.CurrentSpeed / cc.MaxSpeed;
            d[0, rayCount + 1] = rb.angularVelocity.magnitude / rb.maxAngularVelocity;           
            return d;
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