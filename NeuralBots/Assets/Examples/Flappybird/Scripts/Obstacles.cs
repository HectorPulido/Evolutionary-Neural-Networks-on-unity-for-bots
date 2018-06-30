using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public float speed;
    public Vector2 yRange = new Vector2(5, -1.3f);
    public float jumpPostion = 11;
    public Transform center;
    public Vector3 startPoint;

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
            pos.y = Random.Range(yRange.x,yRange.y);
            transform.position = pos;

        }
    }
    public void ReturnToStart()
    {
        transform.position = startPoint;
    }

}
