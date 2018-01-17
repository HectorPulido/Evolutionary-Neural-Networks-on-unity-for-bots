using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    Scenario scenario;

    void Start()
    {
        scenario = transform.parent.parent.GetComponent<Scenario>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ball"))
        {
            if (col.GetComponent<Ball>().inv)
            {
                ArkanoidMendelMachine.current.AddFitness(scenario.id);
                Destroy(gameObject);
            }
            col.SendMessage("InverRbVelocityY");

        }
    }
}
