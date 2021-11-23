using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChat : MonoBehaviour
{

    public GameObject ChatWindow;
    

    public void OnClickChat()
    {
        if (ChatWindow.activeInHierarchy == true)
        {
            ChatWindow.SetActive(false);
        }
        else
        {
            ChatWindow.SetActive(true);
        }
    }
}
