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
        public GameObject cardPrefab;

        public Text indicesText;

        [Tooltip("List of all card images")]
        [SerializeField]
        public Sprite[] Sprites;

        [Tooltip("The local players deck area")]
        [SerializeField]
        GameObject localPlayerDeck;

        [Tooltip("Used for adding decks that represent remote players in-game")]
        [SerializeField]
        GameObject[] remotePlayers;

        [Tooltip("The area where remote players are added to on the game table")]
        [SerializeField]
        GameObject remotePlayerRegion;


        [Tooltip("Represents remote players in the game and the players info")]
        [SerializeField]
        GameObject remotePlayerInfo;

        [Tooltip("The player decks in order of how they appear where 0")]
        [SerializeField]
        List<GameObject> playerDeck = new List<GameObject>();
        #endregion

        #region Private Fields
        
        private List<int> deckIndices = new List<int>();
        List<int> taken = new List<int>();

        #endregion

        #region Public Fields
        public List<UnPlayer> Players = new List<UnPlayer>();

        public int localPlayer;

        public Player currentTurn;
        public int turn = 0;
        public int direction = 1;
        public int playerIndex;
        public List<GameObject> discardPile = new List<GameObject>();
        #endregion

        #region Public Static Fields

        public static GameManager gameManager;

        #endregion

        #region MonoBehavior Methods

        // Start is called before the first frame update
        void Start()
        {
            gameManager = this; 
            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {
                UnPlayer unPlayer = new UnPlayer(players[i], i);
                Players.Add(unPlayer);
                if (players[i].IsMasterClient)
                    localPlayer = i;
            }
            //Camera.main.depthTextureMode = DepthTextureMode.Depth;
            if (PhotonNetwork.IsConnected)
            {
                startGame();
            }
            
        }

        #endregion

        #region Private Functions CardGeneration

        //private List<GameObject> generateCards(Player player, GameObject parent, int owner)
        //{
        //    List<GameObject> generatedCards = new List<GameObject>();
        //    for (int i = 0; i < 7; i++)
        //    {
        //        GameObject cardGo = Instantiate(cardPrefab, parent.transform);
        //    redo:
        //        int cI = Random.Range(0, Sprites.Length);
        //        if (taken.Contains(cI))
        //            goto redo;
        //        cardGo.GetComponent<SpriteRenderer>().sprite = Sprites[cI];
        //        deckIndices.Add(cI);
        //        Vector3 v3 = cardGo.GetComponent<Transform>().position;
        //        cardGo.GetComponent<Card>().setOwner(player, owner);
        //        cardGo.GetComponent<Card>().setPosition(i);
        //        Vector3 newV3 = new Vector3(-300 + i * 100, v3.y + 150, 0 - i * 2);
        //        cardGo.GetComponent<Transform>().localPosition = newV3;
        //        generatedCards.Add(cardGo);
        //    }
        //    return generatedCards;
        //}

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
            int position = 0;
            foreach (int i in indexes)
            {
                GameObject cardGo = createCard(i, player, owner, parent);
                cardGo.GetComponent<Card>().setPosition(position);
                Vector3 localPosition = cardGo.GetComponent<Transform>().localPosition;
                Vector3 newV3 = new Vector3(localPosition.x, localPosition.y, 0 - position * 2);
                //if (PhotonNetwork.LocalPlayer != player)
                //{
                //    newV3 = new Vector3(-300 + j * 100, v3.y - 150, 0 - position * 2);
                //}
                //else
                //{
                //    newV3 = new Vector3(-300 + j * 100, v3.y + 150, 0 - position * 2);
                //}
                cardGo.GetComponent<Transform>().localPosition = newV3;
                generatedCards.Add(cardGo);
                position++;
            }
            return generatedCards;
        }

        public GameObject createCard(int index, Player owner, int ownerId, GameObject parent)
        {
            GameObject cardGo = Instantiate(cardPrefab, parent.transform);
            cardGo.GetComponent<SpriteRenderer>().sprite = Sprites[index];
            Vector3 v3 = cardGo.GetComponent<Transform>().position;
            cardGo.GetComponent<Card>().setOwner(UnPlayer.getUnPlayer(owner));
            return cardGo;
        }
        #endregion

        #region Public Methods

        public void startGame()
        {
            Player[] players = PhotonNetwork.PlayerList;
            //for(int i = 0; i < players.Length; i++)
            //{
            //    Player player = players[i];
            //    Players.Add(player, new List<Card>());
            //    if (player == PhotonNetwork.LocalPlayer)
            //        playerIndex = i;
            //}
            //Players.p(PhotonNetwork.PlayerList, new List<Card>());
            if (PhotonNetwork.IsMasterClient)
            {
                //int[] masterIndices = generateCardIndices();
                //sendGeneratedCards(ToObjArr(masterIndices), players[0], RpcTarget.All, playerIndex);
                //for (int i = 0; i < Players.Count; i++)
                //{
                //    if (players[i].IsMasterClient)
                //        continue;
                //    else
                //    {
                //        int[] indices = generateCardIndices();
                //        Debug.Log("Sending cards...");
                //        foreach (int index in indices)
                //            Debug.Log($"Index: {index}");
                //        sendGeneratedCards(ToObjArr(indices), players[i], RpcTarget.All, i);
                //    }
                //}
                for(int i = 0; i < players.Length; i++)
                {
                    sendGeneratedCards(ToObjArr(generateCardIndices()), players[i], RpcTarget.All, i);
                }
            }

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
            photonView.RPC("receiveCards", target, player, indices, PlayerIndex);
        }

        #endregion

        #region PunRPC
        [PunRPC]
        void receiveCards(Player player, object[] indices, int playerIndex)
        {
            List<int> indexList = new List<int>();
            foreach (object obj in indices)
            {
                indexList.Add((int)obj);
            }

            if (player == PhotonNetwork.LocalPlayer)
            {
                List<GameObject> cards = generateCards(player, localPlayerDeck, indexList.ToArray(), playerIndex);
                UnPlayer unPlayer = UnPlayer.getUnPlayer(player);
                unPlayer.addCards(CardInfo.getCards(cards));
                Players[playerIndex] = unPlayer;
            }
            else if (!player.Equals(PhotonNetwork.LocalPlayer))
            {
                //generateCards(player, remotePlayers[remotePlayers.Length - 1], indexList.ToArray(), Players.FindIndex(p => p == player));
                if(remotePlayerInfo != null && remotePlayerRegion != null)
                {
                    UnPlayer unPlayer = UnPlayer.getUnPlayer(player);
                    if (unPlayer != null)
                    {
                        GameObject RemotePlayerInfo = Instantiate(remotePlayerInfo, remotePlayerRegion.transform);
                        unPlayer.addCards(CardInfo.generateCardList(indexList, unPlayer, 0));
                        Players[playerIndex] = unPlayer;
                    }
                }
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
