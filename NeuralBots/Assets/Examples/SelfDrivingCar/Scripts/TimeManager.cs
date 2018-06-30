using UnityEngine;
namespace Evolutionary_perceptron.Util
{
    public class TimeManager : MonoBehaviour
    {
        public float timeMult = 2;
        float initialTime;       

        void Start()
        {
            initialTime = Time.timeScale;
            InvokeRepeating("TimeUpdate", 0, 0.5f);
        }
        void TimeUpdate()
        {
            Time.timeScale = initialTime * timeMult;
        }
    }
}
