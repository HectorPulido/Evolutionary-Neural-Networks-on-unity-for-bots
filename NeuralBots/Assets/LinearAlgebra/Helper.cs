using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace LinearAlgebra
{
    public static class Helper
    {
        public static bool SaveMatrix(Matrix m, string directory)
        {
            FileStream fs = new FileStream(directory, FileMode.Create);
            
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, m.matrix);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serializate: " + e.Message);
                return false;
            }
            finally
            {
                fs.Close();
            }
            return true;
        }
        public static bool LoadMatrix(out Matrix m, string directory)
        {
            FileStream fs = new FileStream(directory, FileMode.Open);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                m = (double[,])bf.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserializate: " + e.Message);
                m = null;
                return false;
            }
            finally
            {
                fs.Close();
            }
            return true;
        }
        public static bool SaveMatrix(Matrix[] m, string directory)
        {
            FileStream fs = new FileStream(directory, FileMode.Create);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                double[][,] _m = new double[m.Length][,];
                for (int i = 0; i < m.Length; i++)
                {
                    _m[i] = m[i].matrix;
                }

                bf.Serialize(fs, _m);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serializate: " + e.Message);
                return false;
            }
            finally
            {
                fs.Close();
            }
            return true;
        }
        public static bool LoadMatrix(out Matrix[] m, string directory)
        {
            try
            {
                FileStream fs = new FileStream(directory, FileMode.Open);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    double[][,] _m = (double[][,])bf.Deserialize(fs);
                    m = new Matrix[_m.Length];

                    for (int i = 0; i < _m.Length; i++)
                    {
                        m[i] = _m[i];
                    }
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserializate: " + e.Message);
                    m = null;
                    return false;
                }
                finally
                {
                    fs.Close();
                }
            }
            catch (Exception)
            {
                m = null;
                return false;
            }

            return true;
        }
        public static bool ReadCsv(out string[][]data, string directory, char separator = ';')
        {
            string file = "";
            try
            {
                file = File.ReadAllText(directory).Replace("\r", "").Replace(".", ",");
            }
            catch
            {
                data = null;
                return false;
            }
            
            var columns = file.Split(Environment.NewLine.ToCharArray());
            data = new string[columns.Length][];

            for (int i = 0; i < columns.Length; i++)
            {
                data[i] = columns[i].Split(separator);
            }
            return true;
        }
        public static bool SaveCsv(string[][] data, string directory, char separator = ';')
        {
            string[] s = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    s[i] += data[i][j] + separator;
                }
            }
            try
            {
                File.WriteAllLines(directory, s);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static void MapCsv(ref string[][]data, Dictionary<string, string> mapping)
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    foreach (var p in mapping.Keys)
                    {
                        data[i][j] = data[i][j].Replace(p, mapping[p]);
                    }
                }
            }
        }
        public static string[][] MatrixToCsv(Matrix m)
        {
            double[,] _m = m;
            string[][] output = new string[m.x][];
            for (int i = 0; i < m.x; i++)
            {
                output[i] = new string[m.y];
                for (int j = 0; j < m.y; j++)
                {
                    output[i][j] = _m[i, j].ToString();
                }                
            }
            return output;
        }
        public static Matrix CsvToMatrix(string[][] data)
        {
            double[,] m = new double[data.Length, data[0].Length];

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (!double.TryParse(data[i][j], out m[i, j]))
                        m[i, j] = 0;
                }
            }
            return m;
        }
    }
}
