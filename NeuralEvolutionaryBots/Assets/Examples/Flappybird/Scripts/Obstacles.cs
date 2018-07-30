using UnityEngine;
namespace EvolutionaryPerceptron.Examples.FlappyBird
{
    public class Obstacles : MonoBehaviour
    {
        public float speed;
        public Vector2 yRange = new Vector2(5, -1.3f);
        public float jumpPostion = 11;
        public Transform center;

        Vector3 startPoint;

        private void Start()
        {
            startPoint = transform.position;
        }

        private void Update()
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x < -6)
            {
                var pos = transform.position + Vector3.right * jumpPostion;
                pos.y = Random.Range(yRange.x, yRange.y);
                transform.position = pos;
            }
        }
        public void ReturnToStart()
        {
            transform.position = startPoint;
        }
    }
}