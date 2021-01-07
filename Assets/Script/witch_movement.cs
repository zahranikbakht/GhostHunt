using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class witch_movement : MonoBehaviour
{


    public int speed;
    public bool dead;
    // Start is called before the first frame update
    void Start()
    {
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
       //sin movement on the y axis to simulate the witch movement
        transform.position += new Vector3(Mathf.Min((speed + (Game_Manager.LevelNumber / 8.0f)), 10.0f) * Time.deltaTime, Mathf.Sin(Time.time) * Time.deltaTime, 0);

        // if the witch is out of the screen, destroy it
        if (transform.position.x > 12)
        {
            Game_Manager.PresentEnemies -= 1;
            Object.Destroy(this.gameObject);
        } else if (Game_Manager.shots == 0){
            speed = 15;
            dead = true;

        }
    }
}
