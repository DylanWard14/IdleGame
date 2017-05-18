using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UILeaderboard : MonoBehaviour
{
    public List<int> highscores;
    public Text[] highscoreTexts;

	// Use this for initialization
	void Start ()
    {
        UpdateLeaderboard();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddCurrentScore()
    {
        highscores.Add(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GetCurrentScore());
        UpdateLeaderboard();
    }

    void UpdateLeaderboard()
    {
        var highscoreQuery =
    from score in highscores
        //where score < 10
            orderby score descending
    select score;

        int i = 0;
        foreach (int num in highscoreQuery)
        {
            Debug.Log(num);
            highscoreTexts[i].text = ((i + 1) + ": " + num);
            i++;

            if (i >= 10)
            {
                break;
            }
        }
    }
}
