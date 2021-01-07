using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{

    public static int LevelNumber;  // number of levels passed
    public static int PresentEnemies;   //present enemies at each moment

    public static int shots;    //number of shots left

    public static int round;    //current round number
    public static int RoundPerLevel;    //number of rounds per level
    public static int missed;   //number of targets missed
    private bool IsRoundActive; //whether the round is active
    private bool LevelWon;  //whether the player has met the winning condition

    private int TotalEnemies;   //total enemies expected per round

    //references to target transforms and the canine
    public Transform Pumpkin;  
    public Transform Ghost;
    public Transform Bat;
    public Transform Witch;
    public Transform Canine;
    public Transform CandyFall;

    public static Game_Manager Instance;

    // variables for the witch
    private int[] witch_appreance = { 0, 0 };   //rounds that the witch will appear in
    private int witch_life;
    private bool witch_appear;      //whether a witch is on the scene

    static bool shouldCanineAppear; //upon missing a target or more, this is set to true to allow the canine to appear at the end of the round
    float timer = 0;    //timer for canine

    //decision messages on the screen
    public Text Victory;
    public Text Gameover;

    // variables for the special mode
    float special_timer;
    public static bool special_mode;
    int special_length = 5;
    int special_count = 2;

    //all the audio sources attached to the game object
    AudioSource background_music;
    AudioSource shotgun;
    AudioSource dead;
    AudioSource victory;
    AudioSource lose;
    AudioSource laugh;



    // Start is called before the first frame update
    void Start()
    {

        //variable initialization

        Instance = this;
        LevelNumber = 0;
        PresentEnemies = 0;
        TotalEnemies = 1;
        shots = 3;
        round = 0;
        RoundPerLevel = 10;
        missed = 5;
        IsRoundActive = true;

        witch_appreance[0] = Random.Range(1, 6);
        witch_appreance[1] = Random.Range(6, 11);
        witch_life = 3;
        witch_appear = false;

        shouldCanineAppear = false;
        LevelWon = true;

        special_mode = false;
        special_timer = 0;

        background_music = GetComponents<AudioSource>()[0];
        shotgun = GetComponents<AudioSource>()[1];
        dead = GetComponents<AudioSource>()[2];
        victory = GetComponents<AudioSource>()[3];
        lose = GetComponents<AudioSource>()[4];
        laugh = GetComponents<AudioSource>()[5];

        background_music.Play();

    }

    // Update is called once per frame
    void Update()
    {
        // if the round is active, check for click inputs and spawn enemies when required
        if (IsRoundActive)
        {
            Spawner();
            OnClick();
            CheckSpecial();

        } else
        {

            // if the round is not active, whether the canine should appear or a decision about win/loss should be made
            if (shouldCanineAppear)
            {
                timer += Time.deltaTime;
                if (timer > 1.5 && timer < 3.5)
                {
                    canineAppear();
                }
                else if (timer < 5)
                {
                    canineDisappear();
                }
                else
                {
                    IsRoundActive = true;
                    shouldCanineAppear = false;
                    timer = 0;
                }
            } else if (LevelWon)
            {
                timer += Time.deltaTime;
                CandyFall.position -= new Vector3(0, 3.2f * Time.deltaTime, 0);
                // keep the victory screen for 6.5 seconds then go to the next level
                if (timer > 6.5)
                {
                    victory.Stop();
                    background_music.Play();
                    ResetLevel();
                    Victory.gameObject.SetActive(false);
                    IsRoundActive = true;
                    timer = 0;
                    CandyFall.position = new Vector3(CandyFall.position.x, 6.6f, CandyFall.position.z);
                }
            }
            else
            {
                // keep the gameover screen for 7 seconds then go back to the menu

                timer += Time.deltaTime;
                if (timer > 7)
                {
                    GameOver();
                    
                    timer = 0;
                }
            }
        }

    }

    void Spawner()
    {

        if (special_mode)
        {
            //upon entering the special mode, spawn 4-6 enemies of random types
            if (special_timer == 0)
            {
                TotalEnemies = Random.Range(4, 7);
                for (int i = 0; i < TotalEnemies; i++)
                {
                    int type = Random.Range(1, 4);
                    switch (type)
                    {
                        case 1:
                            GameObject.Instantiate(Pumpkin, new Vector2(Random.Range(-10, 10), 6), Quaternion.identity);
                            break;
                        case 2:
                            GameObject.Instantiate(Ghost, new Vector2(Random.Range(-10, 10), 6), Quaternion.identity);
                            break;
                        case 3:
                            GameObject.Instantiate(Bat, new Vector2(Random.Range(-10, 10), 6), Quaternion.identity);
                            break;
                    }
                    PresentEnemies += 1;
                }
            }
            special_timer += Time.deltaTime;
            
        }

        if (PresentEnemies == 0)
        {
            //if no targets are present, go to the next round
            NextRound();

            if (IsRoundActive)
            {
                //reset the number of shots and spawn 1-2 targets of random types
                shots = 3;
                TotalEnemies = Random.Range(1, 3);

                for (int i = 0; i < TotalEnemies; i++)
                {
                    int type = Random.Range(1, 4);
                    switch (type)
                    {
                        case 1:
                            GameObject.Instantiate(Pumpkin, new Vector2(Random.Range(-10, 10), 6), Quaternion.identity);
                            break;
                        case 2:
                            GameObject.Instantiate(Ghost, new Vector2(Random.Range(-10, 10), 6), Quaternion.identity);
                            break;
                        case 3:
                            GameObject.Instantiate(Bat, new Vector2(Random.Range(-10, 10), 6), Quaternion.identity);
                            break;
                    }
                    PresentEnemies += 1;
                }

                //decide if the witch should appear in this round

                if (!witch_appear && (round == witch_appreance[0] || round == witch_appreance[1]))
                {
                    GameObject.Instantiate(Witch, new Vector2(-11, Random.Range(0, 4)), Quaternion.identity);
                    PresentEnemies += 1;

                }
            }
        }
    }

    void OnClick()
    {

        //check for mouse input (continous for special mode)

        if (IsRoundActive && (Input.GetMouseButtonDown(0) || (special_mode && (Input.GetMouseButton(0)))))
        {
            if (shots > 0 && special_mode == false)
            {

                shots -= 1;
            }

            if (shots >= 0)
            {
                
                shotgun.Play();
                
                //cast a ray from the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
   

                if (hit.transform == null)
                {
                    // if the object hit has no transform do nothing
     
                }
                else if (hit.transform.tag == "Enemy" && !hit.transform.GetComponent<Pumpkin_Movement>().dead)
                {
                    //upon hitting a target, give the appropriate score and destroy the target
                    dead.Play();

                    if (special_mode)
                    {
                        Score_Value.score += 1;

                    }
                    else
                    {
                        Score_Value.score += 3;
                    }

                    //if the hit target is colliding with another target, destory that too and give bonus points
                    if (hit.transform.gameObject.GetComponent<Pumpkin_Movement>().isColliding)
                    {
                        if (special_mode)
                        {
                            Score_Value.score += 6;
                        } else
                        {
                            Score_Value.score += 8;

                        }
                        PresentEnemies -= 1;
                        Destroy(hit.transform.gameObject.GetComponent<Pumpkin_Movement>().otherEnemy);
                    }
                  
                    PresentEnemies -= 1;
                    Destroy(hit.transform.gameObject);
                    

                }
               else if (hit.transform.tag == "Witch" && !hit.transform.GetComponent<witch_movement>().dead)
                {

                    //if a witch is hit, take one life of the witch and give back the bullet
                    witch_life -= 1;
                    if (special_mode == false)
                    {
                        shots += 1;
                    }
                    if (witch_life == 0)
                    {
                        //give 5 bonus points and destory the witch
                        Score_Value.score += 5;
                        Destroy(hit.transform.gameObject);
                        witch_life = 3;
                        PresentEnemies -= 1;
                        witch_appear = false;

                    }


                }


            }

            
        }
        

    }

    void NextRound()
    {
        //upon finishing a round, check if the canine should appear
        if (shouldCanineAppear)
        {
            IsRoundActive = false;


        }
        else
        {


            round += 1;
            witch_appear = false;
            witch_life = 3;

            //if the level is completed, check for winning/losing conditions
            if (round > RoundPerLevel || missed == 0)
            {
                if (round > RoundPerLevel)
                {
                    round = RoundPerLevel;
                }
                IsRoundActive = false;
                background_music.Stop();
                if (missed == 0)
                {
                    if (round <= RoundPerLevel)
                    {
                        round -= 1 ;
                    }
                    lose.Play();
                    Gameover.gameObject.SetActive(true);
                    LevelWon = false;
                }
                else
                {

                    victory.Play();
                    LevelWon = true;
                    Victory.gameObject.SetActive(true);

                }
            }
        }
    }

    void ResetLevel()
    {

        //reset the level variables upon entering the next level
        round = 0;
        LevelNumber += 1;
        missed = 5;

        witch_appreance[0] = Random.Range(1, 6);
        witch_appreance[1] = Random.Range(6, 11);
        witch_life = 3;
        witch_appear = false;

        shouldCanineAppear = false;

        special_count = 2;
    }

    public static void missATarget()
    {
        shouldCanineAppear = true;
        if (missed > 0)
        {
            missed -= 1;
        }
    }
    void GameOver()
    {
        //load the scene menu
        SceneManager.LoadScene(0);
    }

    void canineAppear()
    {
        //move the canine upwards from its initial position
        if (Instance.Canine.position.y < -1.45)
        {
            Instance.Canine.position += new Vector3(0, 2.5f*Time.deltaTime, 0);
        }
        else if (!laugh.isPlaying)
        {
            laugh.Play();
        }
    }

    void canineDisappear()
    {
        //move the canine downwards to hide under the hill

        if (Instance.Canine.position.y > -3.77)
        {
            Instance.Canine.position -= new Vector3(0, 2.5f*Time.deltaTime, 0);
        }
    }

    void CheckSpecial()
    {
        //when pressing space, enter the special mode
        //only twice per level
        if (Input.GetKeyDown("space") && special_mode == false && special_count >0)
        {
            background_music.pitch = 1.5f;
            special_mode = true;
            special_timer = 0;
            special_count -= 1;

        } else if (special_mode && special_timer > special_length)
        {
            background_music.pitch = 1.0f;
            special_mode = false;
            timer = 0;
        }
    }
}


