using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EvolutionaryPerceptron.MendelMachine;

public class DinoManager : MendelMachine {

    public static DinoManager singleton;


    [Header("Manager properties")]
    public TextMeshProUGUI scoreText;

    public float scrollSpeed = 5;
    public float scrollAcceleration = 1;

    public float enemySpeed = 0.5f;
    public float enemyAcceleration = 0.2f;

    private float intialScrollSpeed = 5;
    private float intialEnemySpeed = 0.5f;

    public float score;

    public int index = 0; //Just one way to change the generation

    public float lifeTime = 20;
    public Transform initialPlace;

    protected override void Start () {
        if (singleton != null) {
            Destroy (this);
            return;
        }

        singleton = this;

        intialScrollSpeed = scrollSpeed;
        intialEnemySpeed = enemySpeed;

		base.Start(); 
		StartCoroutine(InstantiateBotCoroutine());
    }

    private void Update () {
        enemySpeed += Time.deltaTime * enemyAcceleration;
        scrollSpeed += Time.deltaTime * scrollAcceleration;

        score += Time.deltaTime;
        scoreText.text = ((int) score).ToString ();
    }

    public void Lose () {
        score = 0;
        scrollSpeed = intialScrollSpeed;
        enemySpeed = intialEnemySpeed;

        var obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");

        foreach (GameObject obstacle in obstacles) {
            Destroy (obstacle);
        }
    }


    //When a bot die
	public override void NeuralBotDestroyed(Brain neuralBot)
	{
        base.NeuralBotDestroyed(neuralBot);

		//Doo some cool stuff, read the examples
		Destroy(neuralBot.gameObject); //Don't forget to destroy the gameObject
		
		index--;
		if (index <= 0)
		{
            Lose();
			Save(); //don't forget to save when you change the generation
			population = Mendelization();
			generation++;
			StartCoroutine(InstantiateBotCoroutine());
		}
	}

    //You can instantiate one, two, what you want
	IEnumerator InstantiateBotCoroutine()
	{
        yield return new WaitForSeconds(0);
		//Instantiate bots
		index = individualsPerGeneration;
		for	(int i = 0 ; i < individualsPerGeneration ; i++)
		{
			var b = InstantiateBot(population[i], lifeTime, initialPlace, i); // A way to instantiate
        }
	}

}