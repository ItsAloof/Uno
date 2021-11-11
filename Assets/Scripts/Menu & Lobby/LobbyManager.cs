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


    private void Start()
    {
        PhotonNetwork.JoinLobby();  // we need to be in a lobby in order to create a room
    }

    public void OnClickCreate()     // when you create a room, you automatically join it
    {
        if (roomCreateInputField.text.Length >= 1)    // checking that name is not empty
        {
            PhotonNetwork.CreateRoom(roomCreateInputField.text, new RoomOptions());   //can add { MaxPlayers = *a number*} after RoomOptions()
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


}
