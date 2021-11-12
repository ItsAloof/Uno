using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomCreateInputField;
    public InputField roomJoinInputField;
    public GameObject LobbyPanel;
    public GameObject RoomPanel;
    public Text roomName;

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;


    private void Start()
    {
        PhotonNetwork.JoinLobby();  // we need to be in a lobby in order to create a room
    }

    public void OnClickCreate()     // when you create a room, you automatically join it
    {
        if (roomCreateInputField.text.Length >= 1)    // checking that name is not empty
        {
            PhotonNetwork.CreateRoom(roomCreateInputField.text, new RoomOptions(){ MaxPlayers = 2 });   //can add { MaxPlayers = *a number*} after RoomOptions()
        }
    }

   
    public void OnClickJoin()
    {
        if (roomJoinInputField.text.Length >= 1)
        {
            PhotonNetwork.JoinRoom(roomJoinInputField.text);
        }
            
    }

    public override void OnJoinedRoom()     // allowed by Pun callbacks, automatically happens on event
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }


    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("Game Room (2 Players)");
    }






}



