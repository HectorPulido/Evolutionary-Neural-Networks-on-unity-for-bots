# Evolutionary Neural Networks on Unity For bots

This is a simple asset that train a neural networks using genetic algorithm in unity to make a bot that can play a game or just interact with the envoriment

## TO DO
- More examples
- Cleaner interface to set the fitness

## HOW IT WORKS
First of anything I will explain the concept of Genetic algorithm: This is a algorithm based on the process of natural selection that works crossovering certains individuals and mutate them with the goal of get a more ideal individuals for a work.

Now the neural network is just a matematical function that can imitate or aproximate to any other matematical function, is the universal approximator. One of this function can be the "Being the most enjoyable final boss" or "Being the most human like chatbot" or "Being the most difficult enemy", so usually the neural network is training using the backpropagation algorithm. But this is a pretty complex algorithm to do, and it does not works when you don't really know the function that you want

Here the Genetic algorithm can help us, just crossover a lot of neural networks and choose the most adapted one

![How it works](https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/blob/master/Images/How%20it%20works.jpg "How it works")


## WHY (MOTIVATION)
I wanted to sell this asset on the store, but Unity ML attacked and I couldn't, so I release it, It's a pretty good and simple experiment, to learn neural networks without backpropagation. <br/>
So this was made for my youtube, and twitch channels <br/>
[![Banner](http://img.youtube.com/vi/ckSKdjosxc8/0.jpg)](http://www.youtube.com/watch?v=ckSKdjosxc8) <br/>
Youtube channel <br/>
https://www.youtube.com/c/HectorAndresPulidoPalmar <br/>
Twitch Channel<br/>
https://www.twitch.tv/hector_pulido_<br/>

## HOW TO USE
Open it on unity 2018 or greater (sorry about that >-< ), is also recomended to set the scripting runtime version in .Net 4.X, you can make your own envoriment and set the fitness function, but I recomend, to look at the examples.

### To create a Envoriment...
You need two things a mendel machine (a trainer if you like the concept), to use it you need to iherit from the class mendel machine and set up, things like the startpoints or the behaviour when the generation is over.

```csharp
using EvolutionaryPerceptron.MendelMachine;

public class MyEnvorimentMendelMachine : MendelMachine
{
	//Init all variables
	protected override void Start()
        {
            base.Start(); 
            StartCoroutine(InstantiateBotCoroutine());
        }	
	//When a bot die
	public override void NeuralBotDestroyed(NeuralBot neuralBot)
        {
            //Doo some cool stuff, read the examples
        }
	//You can instantiate one, two, what you want
	IEnumerator InstantiateBotCoroutine()
        {
            //Instantiate bots
        }
}
```

And you also need a interpreter of the neural bot class this class will act like a Senses and Actuators from the body (the individual), I recomend that the sensors where raycast (o raycast2D) or any lineal information, this class also can change the fitness of the neuralbot.

```csharp
using EvolutionaryPerceptron;

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
![Save and Load](/Images/Save%20and%20load.jpg "Self driving car")<br/>
Now you can select the bot you want to save, and press the save button, that will generate a .nn file, that file is compatible with IMITATION LEARNING, you also can load .nn files by drag and drop, just desactivate the Learning Phase boolean

## EXAMPLES
In this moments there are 4 examples 
### Self driving car
![Self driving car](/Images/SelfDriving%20car.jpg "Self driving car")<br/>

This is the most complete example right now, it can be trained on just 10 generations, the sensors are Raycast and the actuator is the Unity Standard Asset Vehicle Car
The fitnes function is, how many checkpoints it touch.

### Flappy bird bots
![Flappy Bot](/Images/Flappy.gif "Arkanoid Bot") <br/>

This is a flappy bird game bot made with this library, the fitness function is the time, and the Die function is the collision with the tag "Obstacle" <br/> 
Assets from <br/>
https://opengameart.org/content/flappy-beans<br/>
Assets Licence: CC-BY 4.0

### Survival bot
![Survival Bot](/Images/Survival.gif "Survival Bot")<br/>
This is an implementation of the algorithm in the [Survival shooter project from Unity Tec. ](https://www.assetstore.unity3d.com/en/#!/content/40756) <br/>
#### Fitness function
A useful shoot give 2 points<br/>
A fail shoot give -5 points <br/>
When the bot move a lot give 1 point <br/>
When health is lost give -1 points <br/>
##### The project is not in this here, it's in other repository
https://github.com/HectorPulido/Evolutionary-Neural-Bots-On-Survival-Shooter <br/>
Assets Licence: Apache 2.0

### Arkanoid Bot
![Arkanoid Bot](/Images/Arkanoid.jpg?raw=true "Arkanoid Bot")<br/>
This example is not available right now

## This program uses 
1. [Simple Linear Algebra for C#](https://github.com/HectorPulido/Simple_Linear_Algebra?raw=true)

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

## Patreon
Please consider Support on Patreon
- https://www.patreon.com/HectorPulido
