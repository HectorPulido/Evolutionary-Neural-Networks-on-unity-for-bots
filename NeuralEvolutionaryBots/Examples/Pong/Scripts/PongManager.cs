using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
	public class PongManager : MonoBehaviour 
	{
		public Transform  initialRacket;
		public TextMesh textScore;
		public Ball ball;
		public int score1, score2;
		public NeuralRacket currentRacket;
		public Rigidbody2D ballRb, racket;

		// Use this for initialization
		public void Setup()
		{
			score1 = score2 = 0;
			ScoreUpdated();
		}
		void ScoreUpdated () 
		{
			textScore.text = score1+" - "+score2;
			ball.Setup();
		}
		public void AddScore(ScoreZone.Player player)
		{
			if(player == ScoreZone.Player.player1)
			{
				if(currentRacket != null)
					score2 ++;
				ScoreUpdated();
			}
			else if(player == ScoreZone.Player.player2)
			{
				if(currentRacket != null)
					score1 ++;

				ScoreUpdated();
			}
		}

	}
}