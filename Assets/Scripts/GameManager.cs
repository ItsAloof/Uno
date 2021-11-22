using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace Un
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Serialized Fields
        [Tooltip("Card Prefab")]
        [SerializeField]
        GameObject cardPrefab;

        public Text indicesText;

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
        #endregion

        #region Private Fields
        
        private List<int> deckIndices = new List<int>();
        List<int> taken = new List<int>();

        #endregion

        #region Public Fields
        public List<Player> Players = new List<Player>();

        public Player currentTurn;
        public int turn = 0;
        public int playerIndex;
        #endregion

        #region Public Static Fields

        public static GameManager gameManager;

        #endregion

        #region MonoBehavior Methods

        // Start is called before the first frame update
        void Start()
        {
            gameManager = this;
            //Camera.main.depthTextureMode = DepthTextureMode.Depth;
            if (PhotonNetwork.IsConnected)
            {
                startGame();
            }
        }

        #endregion

        #region Private Functions CardGeneration

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
        #endregion

        #region Public Methods

        public void startGame()
        {
            Players.AddRange(PhotonNetwork.PlayerList);
            if (PhotonNetwork.IsMasterClient)
            {
                int[] masterIndices = generateCardIndices();
                sendGeneratedCards(ToObjArr(masterIndices), Players[0], RpcTarget.All, playerIndex);
                for (int i = 0; i < Players.Count; i++)
                {
                    if (Players[i].IsMasterClient)
                        continue;
                    else
                    {
                        int[] indices = generateCardIndices();
                        sendGeneratedCards(ToObjArr(indices), Players[i], RpcTarget.All, i);
                    }
                }
            }
            playerIndex = Players.FindIndex(p => p == PhotonNetwork.LocalPlayer);
        }



        public object[] ToObjArr(int[] indices)
        {
            List<object> objArr = new List<object>();
            foreach(int i in indices)
            {
                objArr.Add(i);
            }
            return objArr.ToArray();
        }

        public void sendGeneratedCards(object[] indices, Player player, RpcTarget target, int PlayerIndex)
        {
            photonView.RPC("receiveCards", target, player, indices);
        }

        #endregion

        #region PunRPC
        [PunRPC]
        void receiveCards(Player player, object[] indices)
        {
            List<int> indexList = new List<int>();
            foreach (object obj in indices)
            {
                indexList.Add((int)obj);
            }

            if (player == PhotonNetwork.LocalPlayer)
            {
                playerDeck.AddRange(generateCards(player, localPlayer, indexList.ToArray(), Players.FindIndex(p => p == player)));
            }
            else if (player != PhotonNetwork.LocalPlayer)
            {
                generateCards(player, remotePlayers[remotePlayers.Length - 1], indexList.ToArray(), Players.FindIndex(p => p == player));
            }
        }
        #endregion

        //public override void OnPlayerLeftRoom(Player otherPlayer)
        //{
        //    if (Players.Contains(otherPlayer))
        //    {
        //        Players.Remove(otherPlayer);
        //    }
        //}

    }
}
