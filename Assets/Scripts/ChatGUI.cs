﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;

namespace Uno
{
    public class ChatGUI : MonoBehaviour, IChatClientListener
    {
        #region Private Fields

        string currentChannel = "general";

        #endregion

        #region Public Fields
        public ChatClient chatClient;
        protected internal ChatAppSettings chatAppSettings;
        public InputField InputFieldChat;
        public GameObject ChatPanel;
        public Text CurrentChannelText;
        public string UserName { get; set; }

        #endregion

        #region Private Serializable Fields

        [Tooltip("The text that displays what username the player is currently connected as")]
        [SerializeField]
        private Text connectedAsText;

        #endregion
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            UserName = PlayerNameInputField.getUserName();

#if PHOTON_UNITY_NETWORKING
            this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
#endif

        }

        // Update is called once per frame
        void Update()
        {
            if(chatClient != null)
            {
                chatClient.Service();
            }
        }

        public void OnApplicationQuit()
        {
            if(chatClient != null)
            {
                chatClient.Disconnect();
            }
        }

        #region Public Methods

        public void OnDestroy()
        {
            if (chatClient != null)
            {
                chatClient.Disconnect();
            }
        }

        public void Connect()
        {
            Debug.Log($"Initiated connection...");
            chatClient = new ChatClient(this);
            chatClient.AuthValues = new AuthenticationValues(UserName);
            chatClient.ConnectUsingSettings(chatAppSettings);
            Debug.Log($"Connected as: {UserName}");
        }

        public void OnEnterSend()
        {
            if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return))
            {
                SendChatMessage(this.InputFieldChat.text);
                InputFieldChat.text = "";
            }
        }

        public void OnClickSend()
        {
            if (InputFieldChat != null)
            {
                SendChatMessage(this.InputFieldChat.text);
                InputFieldChat.text = "";
            }
        }

        void SendChatMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            if (this.chatClient == null)
            {
                Connect();
            }
            this.chatClient.PublishMessage(currentChannel, msg);

        }

        public void DebugReturn(DebugLevel level, string message)
        {
            switch (level)
            {
                case DebugLevel.INFO:
                    Debug.Log($"{level}: {message}");
                    break;

                case DebugLevel.ERROR:
                    Debug.LogError($"{level}: {message}");
                    break;
                case DebugLevel.WARNING:
                    Debug.LogWarning($"{level}: {message}");
                    break;
                default:
                    Debug.Log($"{level}: {message}");
                    break;
            }
        }

        public void OnDisconnected()
        {
            throw new System.NotImplementedException();
        }

        public void OnConnected()
        {
            connectedAsText.text = $"Connected as {PhotonNetwork.NickName}";
            chatClient.SetOnlineStatus(ChatUserStatus.Online);
            chatClient.Subscribe(new string[] { currentChannel });
        }

        public void OnChatStateChange(ChatState state)
        {
            Debug.Log($"Chat State: {state.ToString()}");
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            this.ShowChannel(currentChannel);
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            throw new System.NotImplementedException();
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnsubscribed(string[] channels)
        {
            throw new System.NotImplementedException();
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            throw new System.NotImplementedException();
        }

        public void OnUserSubscribed(string channel, string user)
        {
            throw new System.NotImplementedException();
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            throw new System.NotImplementedException();
        }

        public void ShowChannel(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                return;
            }
            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(channelName, out channel);
            if (!found)
            {
                Debug.Log($"ShowChannel failed to find channel: {channelName}");
                return;
            }
            this.currentChannel = channelName;
            this.CurrentChannelText.text = channel.ToStringMessages();
            Debug.Log($"ShowChannel: {channelName}");
        }

        #endregion
    }
}

