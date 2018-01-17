using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour {

    public int id;
    public GameObject blocksPrefab;

    [HideInInspector]
    public GameObject ball;
    [HideInInspector]
    public Transform startPoint;
    [HideInInspector]
    public Transform blockPoint;

    void Start()
    {
        ball = transform.Find("Ball").gameObject;
        startPoint = transform.Find("StartPoint");
        blockPoint = transform.Find("BlockPoints");
    }
}
