using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Evolutionary_perceptron.NeuralBot;

public class NeuralFlappy : MonoBehaviour
{
    NeuralBot nb;
    Flappy flappy;

	void Start ()
    {
        flappy = GetComponent<Flappy>();
        nb = GetComponent<NeuralBot>();
	}

	void Update ()
    {
        Obstacles o = null;

        var ray = Physics2D.Raycast(transform.position, Vector2.right);
        var n1 = ray.collider != null ? ray.distance : 999999;
        if (ray.collider != null)
        {
            if(ray.collider.CompareTag("Obstacle"))
            ray.collider.GetComponent<Obstacles>();
        }
        ray = Physics2D.Raycast(transform.position, (Vector2.right + Vector2.down).normalized);
        var n2 = ray.collider != null ? ray.distance : 999999;

        if (o == null)
        {
            if (ray.collider != null)
            {
                if (ray.collider.CompareTag("Obstacle"))
                    ray.collider.GetComponent<Obstacles>();
            }
        }

        ray = Physics2D.Raycast(transform.position, (Vector2.right + Vector2.up).normalized);
        var n3 = ray.collider != null ? ray.distance : 999999;

        if (o == null)
        {
            if (ray.collider != null)
            {
                if (ray.collider.CompareTag("Obstacle"))
                    ray.collider.GetComponent<Obstacles>();
            }
        }

        var n4 = transform.position.y;

        var n5 = o == null ? 0 : o.center.position.y;

        var input = new float[1, 5] { {n1, n2, n3, n4, n5} };      

        var output = nb.SetInput(input);

        if (output[0,0] > 0.5f)
        {
            flappy.jumpRequest = true;
        }

        nb.AddFitness(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            nb.Destroy();
        }
    }

}
