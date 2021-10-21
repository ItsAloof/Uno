using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;


namespace Uno
{
    public class ChatGUI : MonoBehaviour
    {
        #region Private Fields

        string connectedAs = "Connected as ";

        #endregion

        #region Private Serializable Fields

        [Tooltip("The text that displays what username the player is currently connected as")]
        [SerializeField]
        private Text connectedAsText;

        #endregion
        // Start is called before the first frame update
        void Start()
        {
            connectedAs += PlayerNameInputField.getUserName();
            connectedAsText.text = connectedAs;
        }

        // Update is called once per frame
        void Update()
        {
            connectedAsText.text = $"Connected as {PhotonNetwork.NickName}";
        }
    }
}

