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
        PhotonNetwork.LeaveLobby();
        if (SceneManager.GetActiveScene().name == "Game Over")
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
