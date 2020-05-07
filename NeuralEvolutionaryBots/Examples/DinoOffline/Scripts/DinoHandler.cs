using System.Collections;
using System.Collections.Generic;
using EvolutionaryPerceptron;
using UnityEngine;

namespace EvolutionaryPerceptron.Examples.Dino {
    [RequireComponent (typeof (DinoPlayer))]
    [RequireComponent (typeof (RaycastLightRadar))]
    public class DinoHandler : BotHandler {
        RaycastLightRadar lightRadar;
        DinoPlayer cs;

        int inputSize;
        //Init all variables
        protected override void Start () {
            base.Start ();
            cs = GetComponent<DinoPlayer> ();
            lightRadar = GetComponent<RaycastLightRadar> ();

            inputSize = lightRadar.numberOfRaycast * 2 + 4 * 2;
            lastInputs = new double[1, inputSize];
        }

        double[, ] lastInputs;
        double[] inputs;
        private void Update () {
            var time = Time.deltaTime;
            var inputs = lightRadar.GetInputs ();
            var currentInput = new double[1, inputSize]; // Sensor info
            for (var i = 0; i < lightRadar.numberOfRaycast; i++) {
                currentInput[0, i] = inputs[i];
            }

            currentInput[0, lightRadar.numberOfRaycast + 0] = cs.rb.velocity.y;
            currentInput[0, lightRadar.numberOfRaycast + 1] = cs.DownArrow ? 1 : 0;
            currentInput[0, lightRadar.numberOfRaycast + 2] = cs.JumpArrow ? 1 : 0;
            currentInput[0, lightRadar.numberOfRaycast + 3] = transform.position.y;

            for (var i = 0; i < lightRadar.numberOfRaycast + 4; i++) {
                currentInput[0, 4 + i + lightRadar.numberOfRaycast] = (currentInput[0, i] - lastInputs[0, i]) * time;
            }

            lastInputs = (double[, ]) currentInput.Clone ();

            var output = nb.SetInput (currentInput); //Feed forward

            if (output[0, 0] > 0.5) // Trigger something
            {
                cs.DownArrow = true;
            }

            if (output[0, 1] > 0.5) // Trigger something
            {
                cs.JumpArrow = true;
            }

            nb.AddFitness (time); // You can reward the lifetime
        }

        private void OnTriggerEnter2D (Collider2D collision) {
            //Example of destroy
            if (collision.CompareTag ("Obstacle")) {
                nb.Destroy ();
            }
        }

    }
}