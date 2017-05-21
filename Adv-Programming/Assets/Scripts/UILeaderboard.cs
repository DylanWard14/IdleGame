using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UILeaderboard : MonoBehaviour
{
    public List<int> highscores; // will hold all the highscores, I will be using a list becuase I do not know how many scores there will be
    public Text[] highscoreTexts; // contains the highscore text objects

    delegate void MyDelegate(); // creates a new delegate
    MyDelegate myDelegate; 

	// Use this for initialization
	void Start ()
    {
        myDelegate = UpdateLeaderboard; // assigning the delegate to use the update leaderboard function
        myDelegate(); // calls the delegated function
        //UpdateLeaderboard();
	}

    // when this button is pressed the score will be added to the scoreboard and the scoreboard will be updated
    public void AddCurrentScore()
    {
        highscores.Add(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GetCurrentScore()); // adds the current score to the scores list
        myDelegate(); // calls the delegated function
    }

    //when the button is pressed the game will quit
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// updates the leaderboard to display the most recent scores
    /// </summary>
    void UpdateLeaderboard()
    {
        var highscoreQuery = // standard Linq query syntax
        from score in highscores // gets the scores in the highscores list
        orderby score descending // orders them in descending order
        select score;

        var highScoreQuery = highscores.OrderByDescending(n => n); // linq Query and lambda expression ordering them in decending order

        int i = 0;
        foreach (int num in highScoreQuery) // loops through all the scores in the query
        {
            //Debug.Log(num);
            highscoreTexts[i].text = ((i + 1) + ": " + num); // changes the text to equal the score
            i++; // increments i

            if (i >= 10) // if we have gone through this 10 times
            {
                break; // exit the loop
            }
        }
    }
}
