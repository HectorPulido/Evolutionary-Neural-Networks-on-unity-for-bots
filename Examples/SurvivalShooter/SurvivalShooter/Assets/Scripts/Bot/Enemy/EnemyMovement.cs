using UnityEngine;
using UnityEngine.AI;

namespace EvolutionaryPerceptron.Examples.Survival.Enemy
{
    using EvolutionaryPerceptron.Examples.Survival;

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        public float damageRange;
        public float damageCadency;
        public int damage;

        Transform target;               // Reference to the player's position.
        BotHealth targetHealth;      // Reference to the player's health.

        Health enemyHealth;        // Reference to this enemy's health.
        NavMeshAgent nav;               // Reference to the nav mesh agent.

        void Awake()
        {
            enemyHealth = GetComponent<Health>();
            nav = GetComponent<NavMeshAgent>();

            InvokeRepeating("Search", 0 , 10);
        }

        void Search()
        {
            target = GetClosest(GameObject.FindObjectsOfType<CustomBotHandler>());
            targetHealth = target.GetComponent<BotHealth>();
        }

        Transform GetClosest(CustomBotHandler[] tar)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (CustomBotHandler t in tar)
            {          
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }
            return tMin;
        }

        float clock;
        void Update()
        {
            if (target == null)
                return;

            // If the enemy and the player have health left...
            if (enemyHealth.currentHealth > 0)
            {
                if (Vector3.Distance(transform.position, target.position) <= damageRange)
                {
                    clock += Time.deltaTime;

                    if (clock > damageCadency)
                    {
                        clock = 0;
                        targetHealth.TakeDamage(-damage);
                    }
                }
                else
                {
                    nav.SetDestination(target.position);
                }
            }
            else
            {
                return;
            }
        }
        void Dead()
        {
            tag = "Untagged";
            Invoke("Destroy", 5);
        }
        private void Destroy()
        {
            Destroy(gameObject);
        }

        void StartSinking() { return; }
    }    
}