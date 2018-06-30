using UnityEngine;
namespace Evolutionary_perceptron.Util
{
    public class CameraAspect : MonoBehaviour
    {
        public float aspect;

        void Start()
        {
            var variance = aspect / Camera.main.aspect;
            if (variance < 1.0f)
                Camera.main.rect = new Rect((1.0f - variance) / 2.0f, 0, variance, 1.0f);
            else
            {
                variance = 1.0f / variance;
                Camera.main.rect = new Rect(0, (1.0f - variance) / 2.0f, 1.0f, variance);
            }
        }
    }
}