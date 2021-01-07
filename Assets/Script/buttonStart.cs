using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonStart : MonoBehaviour
{

    public AudioSource source;
    // Update is called once per frame
    public void StartOnClick()
    {
        source = this.GetComponent<AudioSource>();
        SceneManager.LoadScene(1);
    }
    public void ExitOnClick()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
    public void OnMouseEnter()
    {
        //change the color of the text options to yellow on hover
        source.Play();
        Text txt = transform.Find("Text").GetComponent<Text>();
        txt.color = new Color(1f, 206/255f, 59/255f);
    }
    public void OnMouseExit()
    {
        Text txt = transform.Find("Text").GetComponent<Text>();
        //change the color of the text options to white
        txt.color = new Color(1f, 1f, 1f);

    }
}
