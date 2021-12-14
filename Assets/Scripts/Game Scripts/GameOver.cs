using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject nextGameButton;
    public int count = PhotonNetwork.CurrentRoom.PlayerCount;
    public Text playerCount;
    

   public void OnClickNextGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == count)
        {
            playerCount.text = "Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount;
            nextGameButton.SetActive(true);
        }
        else
        {
            playerCount.text = "A Player Has Left." + PhotonNetwork.CurrentRoom.PlayerCount;
            nextGameButton.SetActive(false);
        }
    }

}
