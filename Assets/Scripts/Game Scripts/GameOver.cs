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

    
    //public void OnGameOver()
    //{
    //    PhotonNetwork.LoadLevel("Game Over");   
    //}

   public void OnClickNextGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            nextGameButton.SetActive(true);
        }
        else
        {
            nextGameButton.SetActive(false);
        }
    }

}
