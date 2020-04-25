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
    }

    double[] lastInputs = new double[7] { 0, 0, 0, 0, 0, 0, 0 };
    double[] inputs;
    void Update () {
        inputs = GetInputs ();

        var i = new double[1, 14] {
            {
            inputs[0], inputs[1], inputs[2], inputs[3], inputs[4], inputs[5], inputs[6],
            lastInputs[0], lastInputs[1], lastInputs[2], lastInputs[3], lastInputs[4], lastInputs[5], lastInputs[6]
            }
        }; // Sensor info

        lastInputs = (double[])inputs.Clone();

        var output = nb.SetInput (i); //Feed forward

        if (output[0, 0] > 0.5) // Trigger something
        {
            cs.DownArrow = true;
        }

        if (output[0, 1] > 0.5) // Trigger something
        {
            cs.JumpArrow = true;
        }

        nb.AddFitness (Time.deltaTime); // You can reward the lifetime
    }

    double[] GetInputs () {

        var ray = Physics2D.Raycast (transform.position, Vector2.right, 999999, 1);
        var n1 = ray.collider != null ? ray.distance : 999999;

        ray = Physics2D.Raycast (transform.position, (Vector2.right + Vector2.down).normalized, 999999, 1);
        var n2 = ray.collider != null ? ray.distance : 999999;

        ray = Physics2D.Raycast (transform.position, (Vector2.right + Vector2.up).normalized, 999999, 1);
        var n3 = ray.collider != null ? ray.distance : 999999;

        ray = Physics2D.Raycast (transform.position, (Vector2.right + Vector2.up * 0.5f).normalized, 999999, 1);
        var n6 = ray.collider != null ? ray.distance : 999999;

        ray = Physics2D.Raycast (transform.position, (Vector2.right + Vector2.up * 0.75f).normalized, 999999, 1);
        var n7 = ray.collider != null ? ray.distance : 999999;


        var n4 = transform.position.y;
        var n5 = transform.position.x;

        return new double[7] { n1, n2, n3, n4, n5, n6, n7 };
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        //Example of destroy
        if (collision.CompareTag ("Obstacle")) {
            nb.Destroy ();
        }
    }

}