using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Un
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Card Prefab")]
        [SerializeField]
        GameObject cardPrefab;

        [Tooltip("List of all card images")]
        [SerializeField]
        public Sprite[] Sprites;

        [Tooltip("The local players deck")]
        [SerializeField]
        GameObject localPlayer;

        [Tooltip("Used for adding decks that represent remote players in-game")]
        [SerializeField]
        GameObject[] remotePlayers;

        [Tooltip("The player decks in order of how they appear where 0")]
        [SerializeField]
        List<GameObject> playerDeck = new List<GameObject>();
        private List<int> deckIndices = new List<int>();

        [SerializeField]
        GameObject playButton;

        public List<Player> Players = new List<Player>();

        public Player currentTurn;
        public int turn = 0;
        public int playerIndex;

        public static GameManager gameManager;

        bool isConnecting;



        static List<GameObject> deck = new List<GameObject>();
        List<int> taken = new List<int>();

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            gameManager = this;
            //Camera.main.depthTextureMode = DepthTextureMode.Depth;
        }

        public void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                if (!PlayerPrefs.HasKey("PlayerName"))
                {
                    PlayerPrefs.SetString("PlayerName", $"Anon#{Random.Range(0, 999)}");
                }

                PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                Debug.Log($"Connecting: {isConnecting}");
            }
            else
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }


        private List<GameObject> generateCards(Player player, GameObject parent, int owner)
        {
            List<GameObject> generatedCards = new List<GameObject>();
            for (int i = 0; i < 7; i++)
            {
                GameObject cardGo = Instantiate(cardPrefab, parent.transform);
            redo:
                int cI = Random.Range(0, Sprites.Length);
                if (taken.Contains(cI))
                    goto redo;
                cardGo.GetComponent<SpriteRenderer>().sprite = Sprites[cI];
                deckIndices.Add(cI);
                Vector3 v3 = cardGo.GetComponent<Transform>().position;
                cardGo.GetComponent<Card>().setOwner(player, owner);
                cardGo.GetComponent<Card>().setPosition(i);
                Vector3 newV3 = new Vector3(-300 + i * 100, v3.y + 150, 0 - i * 2);
                cardGo.GetComponent<Transform>().localPosition = newV3;
                generatedCards.Add(cardGo);
            }
            return generatedCards;
        }

        private int[] generateCardIndices()
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < 7; i++)
            {
            redo:
                int index = Random.Range(0, Sprites.Length);
                if (taken.Contains(index))
                    goto redo;
                taken.Add(index);
                indices.Add(index);
            }
            return indices.ToArray();
        }

        private List<GameObject> generateCards(Player player, GameObject parent, int[] indexes, int owner)
        {
            List<GameObject> generatedCards = new List<GameObject>();
            int j = 0;
            foreach (int i in indexes)
            {
                GameObject cardGo = Instantiate(cardPrefab, parent.transform);
                cardGo.GetComponent<SpriteRenderer>().sprite = Sprites[i];
                Vector3 v3 = cardGo.GetComponent<Transform>().position;
                cardGo.GetComponent<Card>().setOwner(player, owner);
                cardGo.GetComponent<Card>().setPosition(j);
                Vector3 newV3;
                if (PhotonNetwork.LocalPlayer != player)
                {
                    newV3 = new Vector3(-300 + j * 100, v3.y - 150, 0 - j * 2);
                }
                else
                {
                    newV3 = new Vector3(-300 + j * 100, v3.y + 150, 0 - j * 2);
                }
                cardGo.GetComponent<Transform>().localPosition = newV3;
                generatedCards.Add(cardGo);
                j++;
            }
            return generatedCards;
        }



        // Update is called once per frame
        void Update()
        {
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"Successfully connected to Photon as {PhotonNetwork.NickName}. Running Game Version: {PhotonNetwork.GameVersion}, UserId: {PhotonNetwork.LocalPlayer.UserId}");
            if (isConnecting)
            {
                bool room = PhotonNetwork.JoinRandomRoom();
                Debug.Log("Joining room: " + room);
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.Log($"No rooms available creating room now...");
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 });
        }



        public override void OnJoinedRoom()
        {
            Debug.Log($"Joined room successfully!");
            if (!PhotonNetwork.IsMasterClient)
            {
                Players.AddRange(PhotonNetwork.PlayerList);
                playerIndex = Players.Count - 1;
            }
            debugPlayerList();

            if (PhotonNetwork.IsMasterClient)
            {
                playerDeck = generateCards(PhotonNetwork.LocalPlayer, localPlayer, playerIndex);
                Players.Add(PhotonNetwork.LocalPlayer);
                playerIndex = 0;
            }
            else
            {
                this.photonView.ViewID = PhotonNetwork.PlayerList.Length;
            }

            currentTurn = PhotonNetwork.MasterClient;
        }

        private void debugPlayerList()
        {
            //for()
            //for(int i = 0; i < players.Count; i++)
            //{
            //    Debug.Log($"Index: {i} Player: {players..NickName}");
            //}
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"{newPlayer.NickName} has joined! UserId: {newPlayer.UserId}");
            if (Players.Contains(newPlayer))
                return;
            Players.Add(newPlayer);
            debugPlayerList();
            if (PhotonNetwork.IsMasterClient)
            {
                List<object> objects = new List<object>();
                int[] indices = generateCardIndices();
                foreach (int i in indices)
                {
                    objects.Add(i);
                }
                this.gameObject.GetPhotonView().RPC("receiveCards", RpcTarget.All, newPlayer, objects.ToArray());
                generateCards(newPlayer, remotePlayers[0], indices, Players.Count-1);
                objects.Clear();
                foreach (int i in deckIndices)
                {
                    objects.Add(i);
                }
                this.photonView.RPC("receiveCards", RpcTarget.All, PhotonNetwork.LocalPlayer, objects.ToArray());
            }
        }

        [PunRPC]
        void receiveCards(Player player, object[] indices)
        {
            List<int> indexList = new List<int>();
            foreach (object obj in indices)
            {
                indexList.Add((int)obj);
            }

            if (player == PhotonNetwork.LocalPlayer && !PhotonNetwork.IsMasterClient)
            {
                playerDeck.AddRange(generateCards(player, localPlayer, indexList.ToArray(), playerIndex));
            }
            else if (player == PhotonNetwork.MasterClient)
            {
                generateCards(player, remotePlayers[0], indexList.ToArray(), Players.FindIndex(p => p == player));
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (Players.Contains(otherPlayer))
            {
                Players.Remove(otherPlayer);
            }
        }

    }
}
