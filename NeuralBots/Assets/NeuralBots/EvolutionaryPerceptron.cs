using System;
using System.Collections.Generic;
using LinearAlgebra;

namespace Evolutionary_perceptron
{
    [Serializable]
    public struct Perceptron
    {
        public Matrix[] W;
        public int LayerCount { get { return W.Length + 1; } }
        public Genoma genoma { get { return new Genoma(W); } }

        public Perceptron(Genoma genoma)
        {
            W = genoma.W;
        }
        public Perceptron(Random r, int[] NeuronCount)
        {
            W = new Matrix[NeuronCount.Length - 1];
            for (int i = 0; i < W.Length; i++)
            {
                W[i] = Matrix.Random(NeuronCount[i] + 1, NeuronCount[i + 1], r) * 2 - 1;
            }
        }
        public Matrix ForwardPropagation(Matrix InputValue)
        {
            Matrix[] Z = new Matrix[LayerCount];
            Matrix[] A = new Matrix[LayerCount];

            Z[0] = InputValue.AddColumn(Matrix.Ones(InputValue.x, 1));
            A[0] = Z[0];

            for (int i = 1; i < LayerCount; i++)
            {
                Z[i] = (A[i - 1] * W[i - 1]).AddColumn(Matrix.Ones(InputValue.x, 1));
                A[i] = Relu(Z[i]);
            }
            return Z[Z.Length - 1].Slice(0,1, Z[Z.Length - 1].x, Z[Z.Length - 1].y);
        }

        static Matrix Relu(Matrix m)
        {
            double[,] output = m;
            Matrix.MatrixLoop((i, j) => {
                output[i, j] = output[i, j] > 0 ? output[i, j] : 0;
            }, m.x, m.y);
            return output;
        }
        /*
        static Matrix sigmoid(Matrix m)
        {
            double[,] output = m;
            Matrix.MatrixLoop((i, j) => {
                output[i, j] = 1 / (1 + Math.Exp(-output[i, j]));
            }, m.x, m.y);
            return output;
        }
        */
    }

    [Serializable]
    public struct Genoma
    {
        public Matrix[] W;

        public Genoma(Matrix[] W)
        {
            this.W = W;
        }

        public static Genoma Cross(Random r, Genoma parent1, Genoma parent2)
        {
            Matrix[] SonW = new Matrix[parent1.W.Length];

            for (int layer = 0; layer < parent1.W.Length; layer++)
            {
                double[,] w = new double[parent1.W[layer].x, parent1.W[layer].y];
                Matrix.MatrixLoop((i, j) => 
                {
                    if (r.NextDouble() > 0.5)
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
            double mutationRate, double maxPerturbation)
        {
            for (int layer = 0; layer < gen.W.Length; layer++)
            {
                double[,] m = gen.W[layer];
                Matrix.MatrixLoop((i, j) =>
                {
                    if (r.NextDouble() < mutationRate)
                    {
                        m[i,j] += (r.NextDouble() * 2f - 1f) * maxPerturbation;
                    }
                }, gen.W[layer].x, gen.W[layer].y);
                gen.W[layer] = m;
            }
            return gen;
        }
    }

   
}
