using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
	public class AIRacket : MonoBehaviour 
	{
		public Transform ball;
		public float velocity;
		Rigidbody2D rb;

		void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		void FixedUpdate()
		{
			var vel = (ball.position.y - transform.position.y) * velocity;
			vel = Mathf.Clamp(vel, -velocity, velocity);

			rb.velocity = new Vector2(0, vel);

		}
	}
}