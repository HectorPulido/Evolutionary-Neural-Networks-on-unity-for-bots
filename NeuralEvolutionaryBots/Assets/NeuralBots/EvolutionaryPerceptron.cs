using System;
using LinearAlgebra;
namespace EvolutionaryPerceptron
{
    public enum ActivationFunction
    {
        Relu, Sigmoid
    }

    [Serializable]
    public struct Perceptron
    {
        public ActivationFunction activationFunction;
        public FloatMatrix[] W;
        public int LayerCount { get { return W.Length + 1; } }
        public Genoma GetGenoma { get { return new Genoma(W); } }

        public Perceptron(Genoma genoma, 
            ActivationFunction activationFunction)
        {
            this.activationFunction = activationFunction;
            W = genoma.W;
        }
        public Perceptron(Random r, int[] NeuronCount, 
            ActivationFunction activationFunction)
        {
            this.activationFunction = activationFunction;

            W = new FloatMatrix[NeuronCount.Length - 1];
            for (int i = 0; i < W.Length; i++)
            {
                W[i] = FloatMatrix.Random(NeuronCount[i] + 1, NeuronCount[i + 1], r) * 2 - 1;
            }
        }
        public FloatMatrix ForwardPropagation(FloatMatrix InputValue)
        {
            FloatMatrix[] Z = new FloatMatrix[LayerCount];
            FloatMatrix[] A = new FloatMatrix[LayerCount];

            Z[0] = InputValue.AddColumn(FloatMatrix.Ones(InputValue.x, 1));
            A[0] = Z[0];

            for (int i = 1; i < LayerCount; i++)
            {
                Z[i] = (A[i - 1] * W[i - 1]).AddColumn(FloatMatrix.Ones(InputValue.x, 1));

                if (activationFunction == ActivationFunction.Relu)
                {
                    A[i] = Relu(Z[i]);
                }
                else if (activationFunction == ActivationFunction.Sigmoid)
                {
                    A[i] = Sigmoid(Z[i]);
                }                
            }
            return Z[Z.Length - 1].Slice(0,1, Z[Z.Length - 1].x, Z[Z.Length - 1].y);
        }

        static FloatMatrix Relu(FloatMatrix m)
        {
            float[,] output = new float[m.x, m.y];
            FloatMatrix.MatrixLoop((i, j) => {
                output[i, j] = m[i, j] > 0 ? m[i, j] : 0;
            }, m.x, m.y);
            return output;
        }
        
        static FloatMatrix Sigmoid(FloatMatrix m)
        {
            float[,] output = m;
            FloatMatrix.MatrixLoop((i, j) => {
                output[i, j] = 1 / (1 + UnityEngine.Mathf.Exp(-output[i, j]));
            }, m.x, m.y);
            return output;
        }
        
    }

    [Serializable]
    public struct Genoma
    {
        public FloatMatrix[] W;

        public Genoma(FloatMatrix[] W)
        {
            this.W = W;
        }

        public static Genoma Cross(Random r, Genoma parent1, Genoma parent2)
        {
            FloatMatrix[] SonW = new FloatMatrix[parent1.W.Length];

            for (int layer = 0; layer < parent1.W.Length; layer++)
            {
                float[,] w = new float[parent1.W[layer].x, parent1.W[layer].y];
                FloatMatrix.MatrixLoop((i, j) => 
                {
                    if ((float) r.NextDouble() > 0.5)
                    {
                        w[i, j] = parent1.W[layer].GetValue(i, j);
                    }
                    else
                    {
                        w[i, j] = parent2.W[layer].GetValue(i, j);
                    }
                }, parent1.W[layer].x, parent1.W[layer].y);
                SonW[layer] = w;
            }

            return new Genoma(SonW);
        }
        public static Genoma Mutate(Random r, Genoma gen, 
            float mutationRate, float maxPerturbation)
        {
            for (int layer = 0; layer < gen.W.Length; layer++)
            {
                float[,] m = gen.W[layer];
                FloatMatrix.MatrixLoop((i, j) =>
                {
                    if ((float)r.NextDouble() < mutationRate)
                    {
                        m[i,j] += ((float)r.NextDouble() * 2f - 1f) * maxPerturbation;
                    }
                }, gen.W[layer].x, gen.W[layer].y);
                gen.W[layer] = m;
            }
            return gen;
        }
    }

   
}
