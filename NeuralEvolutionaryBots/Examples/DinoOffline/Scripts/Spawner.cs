using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionaryPerceptron.Examples.Dino {

    public class Spawner : MonoBehaviour {
        public GameObject[] prefabs;
        public float timeToInstantiate;
        public float timeVariability;

        IEnumerator Start () {
            while (true) {
                var go = Instantiate (prefabs[Random.Range (0, prefabs.Length)], transform.position, Quaternion.identity);
                yield return new WaitForSeconds (timeToInstantiate + Random.Range (-timeVariability, timeVariability));
            }
        }

    }
}