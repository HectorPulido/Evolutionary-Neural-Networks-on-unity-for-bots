using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionaryPerceptron.Examples.Dino {
    public class MoveLeft : MonoBehaviour {
        public float rangeToDelete = -10;

        void Update () {
            var moveSpeed = DinoManager.singleton.enemySpeed;
            transform.position -= transform.right * Time.deltaTime * moveSpeed;
            if (transform.position.x <= rangeToDelete) {
                Destroy (gameObject);
                return;
            }
        }
    }
}