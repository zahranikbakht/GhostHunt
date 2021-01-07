using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_Value : MonoBehaviour
{

    public static int score;
    public Text textbox;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        textbox = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // set the text for the score label at the bottom of the screen

        textbox.text = "" + score;
    }

    // Reduce the score by one until the score is 0.
    public static void Penaltize()
    {
        // Prevents negative score
        if (score > 0)
        {
            score -= 1;
        }
    }
}
