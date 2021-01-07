using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class bullet_panel : MonoBehaviour
{
    //all the objects in the bullet panel
    private GameObject b1;
    private GameObject b2;
    private GameObject b3;
    private GameObject b4;

    // Start is called before the first frame update
    void Start()
    {
        //the 3 bullets
         b1 = this.transform.GetChild(0).gameObject;
         b2 = this.transform.GetChild(1).gameObject;
         b3 = this.transform.GetChild(2).gameObject;
        // the infinity sign that appears in the special mode
         b4 = this.transform.GetChild(3).gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        //if special mode is activated, deactivate all thebullets and show the infinity sign
        if (Game_Manager.special_mode)
        {
            b1.SetActive(false);
            b2.SetActive(false);
            b3.SetActive(false);
            b4.SetActive(true);

        }
        else
        {
            // deactivate the infinity sign and show the bullets based on the number of shots left
            b4.SetActive(false);


            if (Game_Manager.shots == 3)
            {
                b1.SetActive(true);
                b2.SetActive(true);
                b3.SetActive(true);

            }
            else
            if (Game_Manager.shots == 2)
            {
                b3.SetActive(false);
            }
            else if (Game_Manager.shots == 1)
            {
                b2.SetActive(false);
            }
            else if (Game_Manager.shots == 0)
            {
                b1.SetActive(false);
            }
        }
    }
}
