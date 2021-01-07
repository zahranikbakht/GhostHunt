using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missed : MonoBehaviour
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
        // set the text for the Missed label at the bottom of the screen
        startText.text = "" + Game_Manager.missed;

    }
}
