using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
		public class HumanRacket : MonoBehaviour {

		public float velocity;
		Rigidbody2D rb;

		void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}	

		float v;
		void Update()
		{
			v = Input.GetAxis("Vertical");
		}
		void FixedUpdate()
		{
			rb.velocity = new Vector2(0, v * velocity);
		}
	}
}