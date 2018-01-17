# Evolutionary Neural Networks on Unity For bots

This is a simple asset that train a neural networks using genetic algorithm in unity to make a bot that can play a game or just interact with the envoriment

## TO DO
- More examples
- Cleaner interface to set the fitness

## HOW IT WORKS
First of anything I will explain the concept of Genetic algorithm: This is a algorithm based on the process of natural selection that works crossovering certains individuals and mutate them with the goal of get a more ideal individuals for a work.

Now the neural network is just a matematical function that can imitate or aproximate to any other matematical function, is the universal approximator. One of this function can be the "Being the most enjoyable final boss" or "Being the most human like chatbot" or "Being the most difficult enemy", so usually the neural network is training using the backpropagation algorithm. But this is a pretty complex algorithm to do, and it does not works when you don't really know the function that you want

Here the Genetic algorithm can help us, just crossover a lot of neural networks and choose the most adapted one

![How it works](https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/blob/master/Images/How%20it%20works.png "How it works")


## WHY (MOTIVATION)
I wanted to sell this asset on the store, but Unity ML attacked and I couldn't, so I release it, It's a pretty good and simple experiment, to learn neural networks without backpropagation.

## HOW TO USE
Open it on unity 2017.3 or greater (sorry about that >-< ), you can make your own envoriment and set the fitness function, but I recomend, to look at the examples.

### To create a Envoriment...
You need two things a mendel machine (a trainer if you like the concept), to use it you need to iherit from the class mendel machine and set up, things like the startpoints or the behaviour when the generation is over.

And you also need a interpreter of the neural bot class this class will act like a Senses and Actuators from the body (the individual), I recomend that the sensors where raycast (o raycast2D) or any lineal information, this class also can change the fitness of the neuralbot.

## When you finish to train...
You can save your trainded bot as prefab, make sure you set as false the debug option on the neural bot component. Now you have a full trained bot ready to be proved.

## EXAMPLES
In this moments there are 2 examples 
### Self driving car
![Self driving car](https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/blob/master/Images/SelfDriving%20car.PNG?raw=true "Self driving car")

This is the most complete example right now, it can be trained on just 10 generations, the sensors are Raycast and the actuator is the Unity Standard Asset Vehicle Car

The fitnes function is, how many checkpoints it touch.

### Arkanoid Bot
![Arkanoid Bot](https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots/blob/master/Images/Arkanoid.PNG?raw=true "Arkanoid Bot")

This is a bot that can play Arkanoid, but is not finished yet, I don't really know what the fitness function must be


## This program uses 
1. [Simple Linear Algebra for C#](https://github.com/HectorPulido/Simple_Linear_Algebra?raw=true)

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