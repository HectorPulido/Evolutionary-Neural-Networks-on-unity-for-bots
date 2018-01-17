using Evolutionary_perceptron.NeuralBot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NeuralRacket : MonoBehaviour
{
    Rigidbody2D rb;
    public float Velocity;
    NeuralBot nb;
    Transform ball;

    double[,] inputs;
    double[,] outputs;

    void Start()
    {
        ball = GameObject.FindObjectOfType<Ball>().transform;
        nb = GetComponent<NeuralBot>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        rb.gravityScale = 0;
    }

    void FixedUpdate()
    {
        nb.AddFitness(Time.fixedDeltaTime);

        Vector2 dis = transform.position - ball.position;

        inputs = SensorsInfo();
        inputs[0, sensors.Length] = dis.x;//new double[,] { { dis.x, dis.y, dis.z } };
        inputs[0, sensors.Length + 1] = dis.y;

        outputs = nb.SetInput(inputs);

        rb.velocity = new Vector3(Mathf.Clamp((float)outputs[0, 0], -Velocity, Velocity), 0);	
	}

    public Transform[] sensors;
    public float maxDistance = 100f;
    public LayerMask mask;

    RaycastHit2D rh = new RaycastHit2D();
    double[,] SensorsInfo()
    {
        double[,] Sensors = new double[1, sensors.Length + 2];
        for (int i = 0; i < sensors.Length; i++)
        {
            rh = Physics2D.Raycast(sensors[i].position, sensors[i].up, maxDistance, mask);//(r, out rh, maxDistance);
            if (rh.collider == null)
                Sensors[0, i] = maxDistance;
            else
                Sensors[0, i] = rh.distance;

            Debug.DrawRay(sensors[i].position, sensors[i].up * (float)Sensors[0, i], Color.green, 0.01f);

        }
        return Sensors;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ball"))
        {
            col.SendMessage("SetVelocity", new Vector2(rb.velocity.x, 5));
        }
    }

}
