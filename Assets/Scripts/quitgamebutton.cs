using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitgamebutton : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
