using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaveGame : MonoBehaviour
{
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        
        if (SceneManager.GetActiveScene().name == "Game Over" || SceneManager.GetActiveScene().name == "Game")
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
