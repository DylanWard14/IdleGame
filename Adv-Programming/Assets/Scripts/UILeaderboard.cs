using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UILeaderboard : MonoBehaviour
{
    public List<int> highscores;
    public Text[] highscoreTexts;

    delegate void MyDelegate();
    MyDelegate myDelegate;

	// Use this for initialization
	void Start ()
    {
        myDelegate = UpdateLeaderboard; // assigning the delegate to use the update leaderboard function
        myDelegate(); // calls the delegated function
        //UpdateLeaderboard();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddCurrentScore()
    {
        highscores.Add(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GetCurrentScore());
        myDelegate();
    }

    void UpdateLeaderboard()
    {
        var highscoreQuery = // standard Linq query syntax
        from score in highscores
        orderby score descending
        select score;

        var highScoreQuery = highscores.OrderByDescending(n => n); // linq Query and lambda expression

        int i = 0;
        foreach (int num in highScoreQuery)
        {
            //Debug.Log(num);
            highscoreTexts[i].text = ((i + 1) + ": " + num);
            i++;

            if (i >= 10)
            {
                break;
            }
        }
    }
}
