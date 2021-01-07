using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float parallax_effect;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
   
        transform.position += new Vector3(parallax_effect*Time.deltaTime, 0, 0);
        if (transform.position.x > 20)
        {
            transform.position = new Vector3(-0.5f, transform.position.y, transform.position.z);

        }

    }
}
