using UnityEngine;
using Evolutionary_perceptron;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
namespace Evolutionary_perceptron.MendelMachine
{
    public enum DataManagement
    {
        Save,
        Load,
        SaveAndLoad,
        Nothing
    }
    [System.Serializable]
    public struct Individual
    {
        public Genoma gen;
        public float fitness;
    }
    public class Handler
    {
        public static Individual[] Load(string dataPath, bool useRelativeDataPath, bool debug)
        {
            Individual[] pop;

            if (dataPath == "" || dataPath == null)
                dataPath = useRelativeDataPath ? Application.dataPath + "/Data/data.bin" : "c://data.bin";
            else
                dataPath = (useRelativeDataPath ? Application.dataPath + "/Data" : "") + dataPath;

            if (!File.Exists(dataPath))
                return null;

            using (FileStream fs = new FileStream(dataPath, FileMode.Open))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    pop = (Individual[])formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    if (debug)
                        Debug.Log("Failed to deserialize. Reason: " + e.Message);
                    return null;
                }
                finally
                {
                    fs.Close();
                    if (debug)
                        Debug.Log("Data loaded");
                }
            }

            return pop;
        }

        public static void Save(Individual[] ind, string dataPath, bool useRelativeDataPath, bool debug)
        {
            if (dataPath == "" || dataPath == null)
                dataPath = useRelativeDataPath ? Application.dataPath + "/Data/data.bin" : "c://data.bin";
            else
                dataPath = (useRelativeDataPath ? Application.dataPath + "/Data" : "") + dataPath;


            FileStream fs = new FileStream(dataPath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, ind);
            }
            catch (SerializationException e)
            {
                if (debug)
                    Debug.Log("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                if (debug)
                    Debug.Log("Data saved");
            }
        }
    }
    
}

