using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Round_UI : MonoBehaviour
{
    public Text textbox;
    // Start is called before the first frame update
    void Start()
    {
        textbox = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textbox.text = "" + Mathf.Max(Game_Manager.round,1) +" /" + Game_Manager.RoundPerLevel;
    }

}
