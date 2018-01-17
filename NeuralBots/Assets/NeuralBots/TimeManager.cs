using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    float initialTime;

    public float timeMult = 2;

    void Start ()
    {

        initialTime = Time.timeScale;
        InvokeRepeating("TimeUpdate", 0, 0.5f);
    }
	
	// Update is called once per frame
	void TimeUpdate ()
    {
        Time.timeScale = initialTime * timeMult;

    }
}
