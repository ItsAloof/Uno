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
    
    public Text playerCount;
    public int count;
    public bool iAmCalled = false;
    public void getPlayerCount()
    {
        count = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public void OnClickNextGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    public void Update()
    {
        if (!iAmCalled)
        {
            getPlayerCount();
            iAmCalled = true;
        }
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == count)
        {
            playerCount.text = "Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount;
            nextGameButton.SetActive(true);
        }
        else if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount != count)
        {
            playerCount.text = "A Player Has Left.";
            nextGameButton.SetActive(false);
        }
        else
        {
            playerCount.text = "Wait for Next Game or Leave Game";
        }
    }

}
