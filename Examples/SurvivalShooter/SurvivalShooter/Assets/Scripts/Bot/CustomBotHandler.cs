using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolutionaryPerceptron;

namespace EvolutionaryPerceptron.Examples.Survival
{
    //[ExecuteInEditMode]
    public class CustomBotHandler : BotHandler
    {
        BotMovement pm;
        BotHealth ph;
        BotShooting ps;

        public Transform eyes;
        public int rayCount;
        public int angles;
        public float maxDistance;

        protected override void Start()
        {
            base.Start();

            pm = GetComponent<BotMovement>();
            ph = GetComponent<BotHealth>();
            ps = GetComponentInChildren<BotShooting>();

            lastPos = transform.position;
            InvokeRepeating("Incentive", 0, 2.5f);

            maxDistance = ps.range;

            d = new float[rayCount];
            e = new float[rayCount];

            input = new double[1, d.Length + e.Length + 2];
        }

        Vector3 lastPos;
        void Incentive()
        {
            if (Vector3.Distance(transform.position , lastPos) > 25)
            {
                ModifyFitness(1);
            }
            else
            {
                ModifyFitness(-1);
            }
            lastPos = transform.position;
        }

        double[,] input;
        void Update()
        {
            GetSensors();
            
            for (int i = 0; i < d.Length; i++)
            {
                input[0, i] = d[i];
            }
            for (int i = 0; i < e.Length; i++)
            {
                input[0, i + d.Length] = e[i];
            }

            input[0, d.Length + e.Length] = (double)ph.currentHealth / ph.startingHealth;
            input[0, d.Length + e.Length + 1] = (double)ps.shootCounts / ps.maxShoot;

            var output = nb.SetInput(input);

            pm.h = (float)output[0, 0];
            pm.v = (float)output[0, 1];
            pm.turn = Mathf.Clamp((float)output[0, 2], -1, 1);
            ps.fireButton = output[0, 3] > 0.9f;
        }

        float[] d, e;
        Ray r;
        RaycastHit rh;
        void GetSensors()
        {
            for (int i = 0; i < rayCount; i++)
            {
                float angle = angles * (i - rayCount / 2.0f) / rayCount;
                angle += transform.eulerAngles.y;
                angle *= Mathf.Deg2Rad;

                r = new Ray(eyes.position, new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)));                

                Physics.Raycast(r, out rh, maxDistance);

                if (rh.collider != null)
                {
                    d[i] = rh.distance / maxDistance;
                    if (rh.collider.CompareTag("Player"))
                    {
                        e[i] = 1;
                    }
                    else
                    {
                        e[i] = 0;
                    }
                }
                else
                {
                    d[i] = 1;
                    e[i] = 0;
                }
                               
                if (e[i] == 1)
                {
                    Debug.DrawRay(r.origin, r.direction * d[i] * maxDistance, Color.red);
                }                    
                else
                {
                    Debug.DrawRay(r.origin, r.direction * d[i] * maxDistance, Color.green);
                }
            }
        }
        void ModifyFitness(int damage)
        {
            nb.AddFitness(damage);
        }
        void Dead()
        {
            nb.Destroy();
        }

    }
}