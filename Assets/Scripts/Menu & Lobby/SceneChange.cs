using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneChange : MonoBehaviour
{
    public void OnMultiplayerClick()
    {
        SceneManager.LoadScene("ConnectToServer");
    }

    public void OnBackMainMenuClick()
    {
        SceneManager.LoadScene("Menu");
        PhotonNetwork.Disconnect();
    }


}
