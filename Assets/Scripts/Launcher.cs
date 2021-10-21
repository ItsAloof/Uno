using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Uno
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The UI Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject mainMenu;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;


        [Tooltip("The UI for when a player is connected to a room")]
        [SerializeField]
        private GameObject chatGUI;

        #endregion

        #region Private Fields
        string gameVersion = "1";

        bool isConnecting;

        #endregion

        #region MonoBehavior Callbacks

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            mainMenu.SetActive(true);
            progressLabel.SetActive(false);
            chatGUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region Public Methods

        public void Connect()
        {
            progressLabel.SetActive(true);
            mainMenu.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion

        #region MonoBehaviorPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No rooms available, creating new room now...");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            progressLabel.SetActive(false);
            mainMenu.SetActive(true);
            Debug.Log($"Disconnected due to {cause}");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room successfully!");
            progressLabel.SetActive(false);
            mainMenu.SetActive(false);
            chatGUI.SetActive(true);
        }

        #endregion
    }
}
