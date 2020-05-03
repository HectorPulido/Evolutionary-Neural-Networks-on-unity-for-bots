using System.Collections;
using System.Collections.Generic;
using EvolutionaryPerceptron;
using UnityEngine;

public class DinoHandler : BotHandler {
    public LayerMask mask;
    DinoPlayer cs;
    //Init all variables
    protected override void Start () {
        base.Start ();
        cs = GetComponent<DinoPlayer> ();

        lastInputs = new double[numberOfRaycast];
    }

    double[] lastInputs;
    double[] inputs;
    private void Update () {
        var time = Time.deltaTime;
        var inputs = GetInputs ();
        var currentInput = new double[1, numberOfRaycast * 2 + 2]; // Sensor info
        for (var i = 0; i < numberOfRaycast; i++) {
            currentInput[0, i] = inputs[i];
        }
        for (var i = 0; i < numberOfRaycast; i++) {
            currentInput[0, i + numberOfRaycast] = (inputs[i] - lastInputs[i]) * time;
        }

        currentInput[0, numberOfRaycast * 2] = transform.position.y;
        currentInput[0, numberOfRaycast * 2 + 1] = transform.position.x;

        lastInputs = (double[])inputs.Clone();

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

    public int numberOfRaycast = 12;
    public int offsetAngle = 0;
    public int arcAngle = 180;

    private double[] GetInputs () {


        var inputs = new double[numberOfRaycast];
        var currentRotation = transform.eulerAngles.z;

        for (var i = 0; i < numberOfRaycast; i++) {
            var angle = (float)offsetAngle + (arcAngle * i) / numberOfRaycast;
            angle += currentRotation;
            var direction = Quaternion.AngleAxis (angle, Vector3.forward) * Vector3.right;
            direction = direction.normalized;

            var ray = Physics2D.Raycast (transform.position, direction, 999, mask);
            var length = ray.collider != null ? ray.distance / 999 : 1;

            inputs[i] = length;

            Debug.DrawRay (transform.position, direction * length * 999, Color.green);
        }
        return inputs;
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        //Example of destroy
        if (collision.CompareTag ("Obstacle")) {
            nb.Destroy ();
        }
    }

}