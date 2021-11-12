using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
