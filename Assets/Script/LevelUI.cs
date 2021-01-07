using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    private Text startText;

    // Start is called before the first frame update
    void Start()
    {

        startText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        // set the text for the level label on top of the screen
        startText.text = "LVL " + (Game_Manager.LevelNumber+1);

    }
}

