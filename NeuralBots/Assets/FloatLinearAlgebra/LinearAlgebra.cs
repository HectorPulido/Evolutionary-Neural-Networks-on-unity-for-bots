namespace LinearAlgebra
{
    using System;

    [Serializable]
    public struct FloatMatrix
    {
        public static string dec = "0.00";

        public float[] _matrix;
        public float[,] Matrix {
            get {
                float[,] m = new float[x, y];
                int temp = 0;
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        m[i, j] = _matrix[temp];
                        temp++;
                    }
                }
                return m;
            }
            set {
                this = new FloatMatrix(value);
            } }
        public int x;
        public int y;
        public FloatMatrix T { get { return Transpose(this); } }
        public FloatMatrix Size { get { return  (new float[,] { { x , y } } ); } }
        public FloatMatrix Abs { get { return GetAbs(this); } }
        public float Average { get { return GetAverage(this); } }
        public float Max { get { return GetMax(this); } }
        public FloatMatrix Flat { get { return GetFlat(this); } }
        public float this[int i, int j]
        {
            get { return _matrix[i*x + j]; }
            set { _matrix[i*x + j] = value; }
        }

        //constructor
        public FloatMatrix(int sizex, int sizey)
        {
            x = sizex;
            y = sizey;
            _matrix = new float[sizex * sizey];
        }
        public FloatMatrix(float[,] matrix)
        {
            x = matrix.GetLength(0);
            y = matrix.GetLength(1);
            _matrix = new float[x * y];
            int temp = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    _matrix[temp] = matrix[i, j];
                    temp++;
                }
            }            
        }
        //Setup
        public static implicit operator FloatMatrix(float[,] matrix)
        {
            return new FloatMatrix(matrix);
        }
        public static implicit operator float[,](FloatMatrix matrix)
        {
            return matrix.Matrix;
        }

        //values
        public void SetValue(int x, int y, float value)
        {
            if (_matrix == null)
                throw new ArgumentException("Matrix can not be null");
            _matrix[x * this.x + y] = value;
        }
        public float GetValue(int i, int j)
        {
            if (_matrix == null)
                throw new ArgumentException("Matrix can not be null");
            return _matrix[i + this.x * j];
        }
        public FloatMatrix Slice(int x1, int y1, int x2, int y2)
        {
            if (_matrix == null)
                throw new ArgumentException("Matrix can not be null");

            if (x1 > x2 || y1 > y2 || x1 < 0 || x2 < 0 || y1 < 0 || y2 < 0)
                throw new ArgumentException("Dimensions are not valid");

            float[,] slice = new float[x2 - x1, y2 - y1];

            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    slice[i - x1, j - y1] = Matrix[i, j];
                }
            }
            return  (slice);
        }
        public FloatMatrix Slice(int x, int y)
        {
            return Slice(0,0,x,y);
        }
        public FloatMatrix GetRow(int x)
        {
            if (_matrix == null)
                throw new ArgumentException("Matrix can not be null");

            float[,] row = new float[1, y];
            for (int j = 0; j < y; j++)
            {
                row[0, j] = Matrix[x, j];
            }
            return  (row);
        }
        public FloatMatrix GetColumn(int y)
        {
            if (_matrix == null)
                throw new ArgumentException("Matrix can not be null");

            float[,] column = new float[x, 1];
            for (int i = 0; i < x; i++)
            {
                column[i, 0] = Matrix[i, y];
            }
            return  (column);
        }
        public FloatMatrix AddColumn(FloatMatrix m2)
        {
            if (_matrix == null)
                throw new ArgumentException("Matrix can not be null");
            if (m2.y != 1 || m2.x != x)
                throw new ArgumentException("Invalid dimensions");

            float[,] newMatrix = new float[x, y + 1];
            float[,] m = Matrix;

            for (int i = 0; i < x; i++)
            {
                newMatrix[i, 0] = m2.GetValue(i, 0);
            }
            MatrixLoop((i, j) => 
            {
                newMatrix[i, j + 1] = m[i, j];
            }, x, y);
            return  (newMatrix);
        }
        public FloatMatrix AddRow(FloatMatrix m2)
        {
            if (_matrix == null)
                throw new ArgumentException("Matrix can not be null");
            if (m2.x != 1 || m2.y != y)
                throw new ArgumentException("Invalid dimensions");

            float[,] newMatrix = new float[x + 1, y];
            float[,] m = Matrix;

            for (int j = 0; j < y; j++)
            {
                newMatrix[0, j] = m2.GetValue(0, j);
            }
            MatrixLoop((i, j) =>
            {
                newMatrix[i + 1, j] = m[i, j];
            }, x, y);
            return  (newMatrix);
        }
        //Overriding
        public override string ToString()
        {
            string c = "";
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    c += Matrix[i, j].ToString(dec) + " ";
                }
                c += "\n";
            }
            return c;
        }

        //PREMADES
        public static FloatMatrix Zeros(int x, int y)
        {
            float[,] zeros = new float[x, y];
            MatrixLoop((i, j) => {
                zeros[i, j] = 0;
            }, x, y);
            return  (zeros);
        }
        public static FloatMatrix Ones(int x, int y)
        {
            float[,] ones = new float[x, y];
            MatrixLoop((i, j) => {
                ones[i, j] = 1;
            }, x, y);
            return  (ones);
        }
        public static FloatMatrix Identy(int x)
        {
            float[,] identy = new float[x, x];
            MatrixLoop((i, j) => {
                if (i == j)
                    identy[i, j] = 1;
                else
                    identy[i, j] = 0;
            }, x, x);
            return  (identy);
        }
        public static FloatMatrix Random(int x, int y, Random r)
        {
            float[,] random = new float[x, y];
            MatrixLoop((i, j) => {
                random[i, j] = (float)r.NextDouble();
            }, x, y);
            return random;
        }
        //Operations
        //Transpose
        public static FloatMatrix Transpose(FloatMatrix m)
        {
            float[,] mT = new float[m.y, m.x];
            MatrixLoop((i, j) => {
                mT[j, i] = m.GetValue(i,j);
            }, m.x, m.y);
            return  (mT);
        }
        //ADDITIONS
        public static FloatMatrix operator +(FloatMatrix m1, FloatMatrix m2)
        {
            return MatSum(m1, m2);
        }
        public static FloatMatrix operator +(FloatMatrix m2, float m1)
        {
            return MatfloatSum(m1, m2);
        }
        public static FloatMatrix MatfloatSum(float m1, FloatMatrix m2)
        {
            float[,] a = m2;
            float[,] b = new float[m2.x, m2.y];

            MatrixLoop((i, j) => {

                b[i, j] = a[i, j] + m1;

            }, b.GetLength(0), b.GetLength(1));

            return  (b);
        }
        public static FloatMatrix MatSum(FloatMatrix m1, FloatMatrix m2, bool neg = false)
        {
            if (m1.x != m2.x || m1.y != m2.y)
                throw new ArgumentException("Matrix must have the same dimensions");

            float[,] a = m1;
            float[,] b = m2;
            float[,] c = new float[m1.x,m2.y];
            MatrixLoop((i, j) => {
                if(!neg)
                    c[i, j] = a[i, j] + b[i, j];
                else
                    c[i, j] = a[i, j] - b[i, j];
            }, c.GetLength(0), c.GetLength(1));
            return  (c);
        }
        //SUBSTRACTIONS
        public static FloatMatrix operator -(FloatMatrix m1, FloatMatrix m2)
        {
            return MatSum(m1, m2, true);
        }
        public static FloatMatrix operator -(FloatMatrix m2, float m1)
        {
            return MatfloatSum(-m1, m2);
        }
        //MULTIPLICATIONS
        public static FloatMatrix operator *(FloatMatrix m2, float m1)
        {
            return MatfloatMult(m2, m1);
        }
        public static FloatMatrix operator *(FloatMatrix m1, FloatMatrix m2)
        {
            if (m1.x == m2.x && m1.y == m2.y)
                return DeltaMult(m1,m2);
            return MatMult(m1, m2);
        }
        public static FloatMatrix MatfloatMult(FloatMatrix m2, float m1)
        {
            float[,] a = m2;
            float[,] b = new float[m2.x, m2.y];

            MatrixLoop((i, j) => {

                b[i, j] = a[i, j] * m1;

            }, b.GetLength(0), b.GetLength(1));

            return  (b);
        }
        public static FloatMatrix MatMult(FloatMatrix m1, FloatMatrix m2)
        {
            if (m1.y != m2.x)
                throw new ArgumentException("Matrix must have compatible dimensions");
            int n = m1.x;
            int m = m1.y;
            int p = m2.y;

            float[,] a = m1;
            float[,] b = m2;
            float[,] c = new float[n, p];
            MatrixLoop((i,j) => {
                float sum = 0;
                for (int k = 0; k < m; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                c[i, j] = sum;

            }, n, p);
            return  (c);
        }
        public static FloatMatrix DeltaMult(FloatMatrix m1, FloatMatrix m2)
        {
            if(m1.x != m2.x || m1.y != m2.y)
                throw new ArgumentException("Matrix must have the same dimensions");
            float[,] output = new float[m1.x, m2.y];
            MatrixLoop((i, j) => 
            {
                output[i, j] = m1.GetValue(i, j) * m2.GetValue(i, j);
            }, m1.x, m2.y);
            return  (output);
        }
        //DIVISION
        public static FloatMatrix operator / (FloatMatrix m2, float m1)
        {
            return MatfloatMult(m2, 1 / m1);
        }
        //POW
        public static FloatMatrix operator ^(FloatMatrix m2, float m1)
        {
            return Pow(m2,m1);
        }
        public static FloatMatrix Pow (FloatMatrix m2, float m1)
        {
            float[,] output = new float[m2.x, m2.y];
            MatrixLoop((i, j) => {
                output[i, j] = (float)Math.Pow(m2.GetValue(i, j), m1); 
            }, m2.x, m2.y);
            return  (output);
        }
        public FloatMatrix Pow(float m1)
        {
            return Pow(this, m1);
        }
        //Sumatory 
        public static FloatMatrix Sumatory(FloatMatrix m, int dimension = -1)
        {
            float[,] output;
            if (dimension == -1)
                output = new float[1, 1];
            else if (dimension == 0)
                output = new float[m.x, 1];
            else if (dimension == 1)
                output = new float[1, m.y];
            else
                throw new ArgumentException("The dimension must be -1, 0 or 1");

            if (dimension == -1)
            {
                MatrixLoop((i, j) =>
                {
                    output[0, 0] += m.GetValue(i, j);
                }, m.x, m.y);
            }
            else if (dimension == 0)
            {
                MatrixLoop((i, j) =>
                {
                    output[i, 0] += m.GetValue(i, j);
                }, m.x, m.y);
            }
            else if (dimension == 1)
            {
                MatrixLoop((i, j) =>
                {
                    output[0, j] += m.GetValue(i, j);
                }, m.x, m.y);
            }
            return  (output);
        }
        public FloatMatrix Sumatory(int dimension = -1)
        {
            return Sumatory(this, dimension);
        }
        //DOT PRODUCT
        public FloatMatrix Dot(FloatMatrix m2)
        {
            return Dot(this, m2);
        }
        public static FloatMatrix Dot(FloatMatrix m1, FloatMatrix m2)
        {
            return m1 * m2.T;
        }
        //ABS
        public FloatMatrix GetAbs(FloatMatrix m)
        {
            float[,] d = m;
            MatrixLoop((i, j) => { d[i, j] = Math.Abs(m.GetValue(i, j)); }, m.x, m.y);
            return  (d);
        }
        public float GetAverage(FloatMatrix m)
        {
            float d = 0;
            MatrixLoop((i, j) => { d += m.GetValue(i, j); }, m.x, m.y);
            return d / (m.x * m.y);
        }
        public float GetMax(FloatMatrix m)
        {
            float max = float.MinValue;
            MatrixLoop((i, j) => 
            {
                if (m.GetValue(i, j) > max)
                    max = m.GetValue(i, j);
            }, m.x, m.y);
            return max;
        }

        //Flat
        public FloatMatrix GetFlat(FloatMatrix m)
        {
            float[,] output = new float[m.x * m.y, 1];

            MatrixLoop((i, j) => 
            {
                output[m.x * i + j ,0] = m.GetValue(i,j);
            }, m.x, m.y);

            return output;
        }

        //Handlers
        public static void MatrixLoop(Action<int, int> e, int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    e(i, j);
                }
            }
        }
    }
}
