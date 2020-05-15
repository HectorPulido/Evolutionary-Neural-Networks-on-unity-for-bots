using System;
using LinearAlgebra;

namespace EvolutionaryPerceptron.MendelMachine {
    public enum ActivationFunction { ReLU, Sigmoid }

    [Serializable]
    public struct Perceptron {
        ActivationFunction activationFunction;
        public int LayerCount { get { return W.Length + 1; } }
        public Matrix[] W;

        public Genoma GetGenoma { get { return new Genoma (W); } }

        public Perceptron (Random r, int[] NeuronCount, ActivationFunction activationFunction) {
            this.activationFunction = activationFunction;

            W = new Matrix[NeuronCount.Length - 1];
            for (int i = 0; i < W.Length; i++) {
                W[i] = Matrix.Random (NeuronCount[i] + 1, NeuronCount[i + 1], r) * 2 - 1;
            }
        }
        public Perceptron (Genoma genoma, ActivationFunction activationFunction) {
            this.activationFunction = activationFunction;
            W = genoma.W;
        }

        public Matrix ForwardPropagation (Matrix InputValue) {
            int ExampleCount = InputValue.X;
            var Z = new Matrix[LayerCount];
            var A = new Matrix[LayerCount];

            Z[0] = InputValue.AddColumn (Matrix.Ones (ExampleCount, 1));
            A[0] = Z[0];

            for (int i = 1; i < LayerCount; i++) {
                Z[i] = (A[i - 1] * W[i - 1]).AddColumn (Matrix.Ones (ExampleCount, 1));
                A[i] = Activation (Z[i]);
            }
            var a = Z[Z.Length - 1];
            return a.Slice (0, 1, a.X, a.Y);;
        }

        Matrix Activation (Matrix m) {
            if (activationFunction == ActivationFunction.ReLU) {
                return Relu (m);
            } else if (activationFunction == ActivationFunction.Sigmoid) {
                return Sigmoid (m);
            } else {
                return null;
            }
        }

        Matrix Sigmoid (Matrix m) {
            double[, ] output = m;
            Matrix.MatrixLoop ((i, j) => {
                output[i, j] = 1 / (1 + Math.Exp (-output[i, j]));

            }, m.X, m.Y);
            return output;
        }
        Matrix Relu (Matrix m) {
            double[, ] output = m;
            Matrix.MatrixLoop ((i, j) => {
                output[i, j] = output[i, j] > 0 ? output[i, j] : 0;

            }, m.X, m.Y);
            return output;
        }
    }

    [Serializable]
    public struct Genoma {
        public Matrix[] W;
        public Genoma (Matrix[] W) {
            this.W = (Matrix[]) W.Clone ();
        }

        public static Genoma Cross (Random r, Genoma parent1, Genoma parent2) {
            Matrix[] SonW = new Matrix[parent1.W.Length];

            for (int layer = 0; layer < parent1.W.Length; layer++) {
                double[, ] w = new double[parent1.W[layer].X, parent1.W[layer].Y];
                Matrix.MatrixLoop ((i, j) => {
                    if (r.NextDouble () > 0.5) {
                        w[i, j] = parent1.W[layer].GetValue (i, j);
                    } else {
                        w[i, j] = parent2.W[layer].GetValue (i, j);
                    }
                }, parent1.W[layer].X, parent1.W[layer].Y);
                SonW[layer] = w;
            }

            return new Genoma (SonW);
        }
        public static Genoma Mutate (
            Random r,
            Genoma gen,
            float stdDevMutatuion,
            float maxPerturbation
        ) {
            for (int layer = 0; layer < gen.W.Length; layer++) {
                double[, ] m = gen.W[layer];
                Matrix.MatrixLoop ((i, j) => {
                    m[i, j] += GaussianSystemRandom (r, stdDevMutatuion, maxPerturbation);
                }, gen.W[layer].X, gen.W[layer].Y);
                gen.W[layer] = m;
            }
            return gen;
        }
        public static double GaussianSystemRandom (System.Random r, double stdDev, double height, double mean = 0) {
            double u1 = 1.0 - r.NextDouble (); //uniform(0,1] System.Random doubles
            double u2 = 1.0 - r.NextDouble ();
            double randStdNormal = Math.Sqrt (-2.0 * Math.Log (u1)) * Math.Sin (2.0 * Math.PI * u2); //System.Random normal(0,1)
            return (mean + stdDev * randStdNormal) * height; //System.Random normal(mean,stdDev^2)
        }

    }
}