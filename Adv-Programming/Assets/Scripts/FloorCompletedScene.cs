using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FloorCompletedScene : MonoBehaviour
{
    private float timer; // this timer will count the seconds that have passed
	// Use this for initialization
	void Start ()
    {
        timer = 0; // sets it to 0 at the start of the scene
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime; // starts the timer

        if (timer >= 2) // of the timer is greater than 2
        {
            Application.LoadLevel("Game"); // load the game scene
        }
	}
}
