using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pumpkin_Movement : MonoBehaviour
{

    public float speed = 10.0f; //speed of the target
    public float lifetime = 2;  //lifetime of target in seconds
    public bool timed_out = false;  //whether the target has exceeded its lifetime
    public bool isColliding = false; //whether the target is colliding with any other targets
    public GameObject otherEnemy;   //the gameobject that the target is colliding with
    private Vector2 Followee;   // the random point that the player will follow on the screen to simulate random movement
    public bool dead = false;   //whether the Destroy method is called on the object (since the object won't immediately destroy)

    // Start is called before the first frame update
    void Start()
    {

        Followee = new Vector2(transform.position.x, transform.position.y);

        //set the speed based on the initial speed and the level number
        speed = Mathf.Min((speed + (Game_Manager.LevelNumber / 8.0f)), 10.0f);

        // Call the function leave when lifetime has passed
        Invoke("Leave", lifetime);
    }

    // Update is called once per frame
    void Update()
    {

        //move towards a random point on the screen bounds
        transform.position = Vector2.MoveTowards(transform.position, Followee, speed * Time.deltaTime);

        //if the target has reached the random point that it was moving towards, change the point.
        if (timed_out == false && transform.position.x == Followee.x && transform.position.y == Followee.y && Game_Manager.shots > 0)
        {
            ChangePosition();
        } 
        
        // if the target's lifetime has passed or the player has used all her shots, penaltize the player and destroy the object
        else if ((timed_out == true || Game_Manager.shots == 0) && !dead)
        {
            Score_Value.Penaltize();

            //penaltize twice in special mode
            if (Game_Manager.special_mode)
            {
                Score_Value.Penaltize();
            }

            Game_Manager.missATarget();
            Game_Manager.PresentEnemies -= 1;

            //change the color of the object to red and destory it after 0.5 seconds
            this.GetComponent<SpriteRenderer>().color = new Color(0.6f,0.1f,0.1f);
            Object.Destroy(this.gameObject,0.5f);
            dead = true;
         

        }

    }

    // A function to randomize a point within screen bounds
    void ChangePosition()
    {
       
        //Compute position for next time
        Followee = new Vector2(Random.Range(-10, 10), Random.Range(-3, 5));
    }

    // a function called when target has passed its lifetime
    void Leave()
    {
        Followee = new Vector2(-13, Random.Range(-4, 4));
        timed_out = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if the colliding object is another target, save a reference to it
        if (collision.gameObject.CompareTag("Enemy") && !collision.gameObject.GetComponent<Pumpkin_Movement>().dead)
        {
            isColliding = true;
            otherEnemy = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if the colliding enemy has left
        if (other.gameObject.CompareTag("Enemy"))
        {
            isColliding = false;
        }
    }

}
