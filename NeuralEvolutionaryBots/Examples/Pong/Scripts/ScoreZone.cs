using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvolutionaryPerceptron.Examples.Arkanoid
{
	public class ScoreZone : MonoBehaviour 
	{
		public enum Player {player1, player2}

		public Player player;
		public PongManager pongManager;
		void OnTriggerEnter2D(Collider2D col)
		{
			if(col.CompareTag("Ball"))
			{
				pongManager.AddScore(player);
			}
		}
	}

}