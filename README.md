# Evolutionary Neural Networks on Unity For bots

This is a simple asset that train a neural networks using genetic algorithm in unity to make a bot that can play a game or just interact with the envoriment

## HOW IT WORKS

[![Flappy bird bot](https://img.youtube.com/vi/9BIe80WGhxY/0.jpg)](https://www.youtube.com/watch?v=9BIe80WGhxY)

First of anything I will explain the concept of Genetic algorithm: This is a algorithm based on the process of natural selection that works crossovering certains individuals and mutate them with the goal of get a more ideal individuals for a work.

Now the neural network is just a matematical function that can imitate or aproximate to any other matematical function, is the universal approximator. One of this function can be the "Being the most enjoyable final boss" or "Being the most human like chatbot" or "Being the most difficult enemy", so usually the neural network is training using the backpropagation algorithm. But this is a pretty complex algorithm to do, and it does not works when you don't really know the function that you want

Here the Genetic algorithm can help us, just crossover a lot of neural networks and choose the most adapted one

![How it works](https://raw.githubusercontent.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/master/Images/How%20it%20works.jpg "How it works")


## WHY (MOTIVATION)
I wanted to sell this asset on the store, but Unity ML attacked and I couldn't, so I release it, It's a pretty good and simple experiment, to learn neural networks without backpropagation. <br/>
So this was made for my youtube, and twitch channels <br/>
[![Banner](http://img.youtube.com/vi/ckSKdjosxc8/0.jpg)](http://www.youtube.com/watch?v=ckSKdjosxc8) <br/>
Youtube channel <br/>
https://www.youtube.com/c/HectorAndresPulidoPalmar <br/>
Twitch Channel<br/>
https://www.twitch.tv/hector_pulido_<br/>

## TO DO
- More examples
- Cleaner interface to set the fitness

## HOW TO USE
Open it on unity 2018 or greater (sorry about that >-< ), is also recomended to set the scripting runtime version in .Net 4.X, you can make your own envoriment and set the fitness function, but I recomend, to look at the examples.

### To create a Envoriment...
You need two things a mendel machine (a trainer if you like the concept), to use it you need to iherit from the class mendel machine and set up, things like the startpoints or the behaviour when the generation is over.

```csharp
using EvolutionaryPerceptron.MendelMachine;

public class ExampleMendelMachine : MendelMachine {

	int index = 0; //Just one way to change the generation
	//Init all variables
	protected override void Start()
	{
		individualsPerGeneration = somenumber; //You can set an individuals per generation here
		base.Start(); 
		StartCoroutine(InstantiateBotCoroutine());
	}	
	//When a bot die
	public override void NeuralBotDestroyed(Brain neuralBot)
	{
		//Consolidate the fitness
        base.NeuralBotDestroyed(neuralBot);

		//Doo some cool stuff, read the examples
		Destroy(neuralBot.gameObject); //Don't forget to destroy the gameObject
		
		index--;
		if (index <= 0)
		{
			Save(); //don't forget to save when you change the generation
			population = Mendelization();
			generation++;
			StartCoroutine(InstantiateBotCoroutine());
		}
	}
	//You can instantiate one, two, what you want
	IEnumerator InstantiateBotCoroutine()
	{
		//Instantiate bots
		index = individualsPerGeneration;
		for	(int i = 0 ; i < individualsPerGeneration ; i++)
		{
			var b = InstantiateBot(population[i], lifeTime, someTransform, i); // A way to instantiate
		}
	}
}
```

And you also need a interpreter of the neural bot class this class will act like a Senses and Actuators from the body (the individual), I recomend that the sensors where raycast (o raycast2D) or any lineal information, this class also can change the fitness of the neuralbot.

```csharp
public class NeuralExample : BotHandler
{
	MyControllerClass cs;
	//Init all variables
	protected override void Start()
	{
		base.Start();
		cs = GetComponent<MyControllerClass>();
	}	
	void Update()
	{
		var i = new double[1, 5] { { n1, n2, n3, n4, n5 } }; // Sensor info
		var output = nb.SetInput(i); //Feed forward
		
		cs.speed = output[0, 0]; // Linear something

            	if (output[0, 1] > 0.5) // Trigger something
            	{
                	cs.jumpRequest = true;
            	}
		nb.AddFitness(Time.deltaTime); // You can reward the lifetime
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Example of destroy
		if (collision.CompareTag("Obstacle"))
		{
			nb.Destroy();
		}
	}

}
```

### When you finish to train...
Now you can select the bot you want to save, and press the save button, that will generate a .nn file, that file is compatible with IMITATION LEARNING, just desactivate the Learning Phase boolean

## EXAMPLES
In this moments there are 4 examples 
### Self driving car
![Self driving car](https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/blob/master/Images/SelfDriving%20car.jpg "Self driving car")<br/>

An automatic Car in unity, it can be trained on just 10 generations, the sensors are Raycast and the actuator is the Unity Standard Asset Vehicle Car

#### Fitness function
The fitnes function is, how many checkpoints it touch and the Die function is the collision with the tag "Obstacle"

### Flappy bird bots
![Flappy Bot](https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/blob/master/Images/Flappy.gif) <br/>
This is a flappy bird game bot made with this library, the sensors are 3 raycast, the position, and the center of the obstacles<br/>


### Dinosaur clone bots
![Dinosaur](https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/blob/master/Images/DinoChrome.gif) <br/>
This is a google chrome clone, it have 7 raycast as sensor aditional to the 7 raycast of the last frame, and 2 buttons as outputs also the fitness function is the lifetime

#### Fitness function
The fitness function is the time, and the Die function is the collision with the tag "Obstacle" <br/> 
Assets from <br/>

### Asteroids like game
![Asteroids like game](https://github.com/HectorPulido/Asteroids-like-game/raw/master/img/img_ml.gif)<br/>
This is an implementation of the algorithm for the Asteroid game; The sensors are a lot of raycast that detect asteroids, and the die function is when ship collide with an asteroid, and there are 3 output, one for shoot, one for turn, other for accelerate

#### Fitness function
Lifetime without shoot is a positive reward<br/>
Shooting time is a negative reward <br/>

#### The project is not in this here, it's in other repository 
https://github.com/HectorPulido/Asteroids-like-game <br/>
Assets Licence: MIT

### Survival bot
![Survival Bot](https://raw.githubusercontent.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/master/Images/Survival.gif "Survival Bot")<br/>
This is an implementation of the algorithm in the [Survival shooter project from Unity Tec. ](https://www.assetstore.unity3d.com/en/#!/content/40756) 
The sensors are a lot of raycast that detect shootables, and enemies, and the die function is when the life gets 0 

#### Fitness function
A useful shoot give 2 points<br/>
A fail shoot give -5 points <br/>
When the bot move a lot give 1 point <br/>
When health is lost give -1 points <br/>

#### The project is not in this here, it's in other repository 
https://github.com/HectorPulido/Evolutionary-Neural-Bots-On-Survival-Shooter <br/>
Assets Licence: Apache 2.0

### PONG Bot
![Pong Bot](https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/blob/master/Images/Pong.gif "Pong Bot")<br/>
This is an implementation for the pong game, the sensors are the position and velocity of the ball, the the position and the velocity of the enemy racket and the position of the racket (All the position must be locals) 
#### Fitness function
Pro point add one of fitness point <br/>
Contra point remove one of fitness point <br/>

## This program uses 
1. [Simple Linear Algebra for C#](https://github.com/HectorPulido/Simple_Linear_Algebra)

## OTHER WORKS 
### IMITATION LEARNING IN UNITY
This is an open source project that uses neural networks and backpropagation in C#, and train it via stochastic gradient descend using the human behaviour as base <br/>
https://github.com/HectorPulido/Imitation-learning-in-unity
### More Genetic algorithms on Unity
Those are three Genetics Algorithm using unity, The First one is a simple algorithm that Looks for the minimun of a function, The Second one is a solution for the Travelling Salesman Problem, The Third one is a Automata machine <br/>
https://github.com/HectorPulido/Three-Genetics-Algorithm-Using-Unity
### Vectorized Multilayer Neural Network from scratch
This is a simple MultiLayer perceptron made with Simple Linear Algebra for C# , is a neural network based on This Algorithm but generalized. This neural network can calcule logic doors like Xor Xnor And Or via Stochastic gradient descent backpropagation with Sigmoid as Activation function, but can be used to more complex problems. <br/>
https://github.com/HectorPulido/Vectorized-multilayer-neural-network

## Where can i learn more
- On my Youtube channel (spanish) are a lot of information about Machine learning and Neural networks
- https://www.youtube.com/channel/UCS_iMeH0P0nsIDPvBaJckOw
- This convolutional neural network (Pretty hardcore)
- https://github.com/HectorPulido/Convolutional-Neural-Network-From-Scratch
- This example of Genetics Algorithm on unity
- https://github.com/HectorPulido/Three-Genetics-Algorithm-Using-Unity
- You can also look at the Multilayer perceptron 
- https://github.com/HectorPulido/Vectorized-multilayer-neural-network
- Or The monolayer Example (Simpler)
- https://github.com/HectorPulido/Simple-vectorized-mono-layer-perceptron
- Or Look at a Non Vectorized multilayer perceptronExample
- https://github.com/HectorPulido/Multi-layer-perceptron

