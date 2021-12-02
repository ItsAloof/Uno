﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

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
        public GameObject localPlayerDeck;

        [Tooltip("Used for adding decks that represent remote players in-game")]
        [SerializeField]
        GameObject[] remotePlayers;

        [Tooltip("The area where remote players are added to on the game table")]
        [SerializeField]
        GameObject remotePlayerRegion;


        [Tooltip("Represents remote players in the game and the players info")]
        [SerializeField]
        GameObject remotePlayerInfo;

        [Tooltip("The players decks in order of how they appear where 0")]
        [SerializeField]
        public List<GameObject> playerDeck = new List<GameObject>();
        #endregion

        #region Private Fields

        private List<int> deckIndices = new List<int>();
        [SerializeField]
        List<int> taken = new List<int>();

        #endregion

        #region Public Fields

        public List<UnPlayer> Players = new List<UnPlayer>();

        public int localPlayer;

        public Player currentTurn;
        public int turn = 0;
        public int direction = 1;
        public List<GameObject> discardPile = new List<GameObject>();

        public List<GameObject> localPlayerCards = new List<GameObject>();
        public List<GameObject> remotePlayerCards = new List<GameObject>();

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
                else if (players[i] == PhotonNetwork.LocalPlayer)
                {
                    localPlayer = i;
                }

            }
            if (PhotonNetwork.IsConnected)
            {
                startGame();
            }

        }

        #endregion

        #region Private Functions CardGeneration

        private int[] generateCardIndices()
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < 7; i++)
            {
            redo:
                int index = UnityEngine.Random.Range(0, Sprites.Length);
                if (taken.Contains(index))
                    goto redo;
                taken.Add(index);
                indices.Add(index);
            }
            return indices.ToArray();
        }

        private List<GameObject> generateCards(Player player, GameObject parent, int[] indexes, int owner, int index = 0)
        {
            List<GameObject> generatedCards = new List<GameObject>();
            int position = index;
            foreach (int i in indexes)
            {
                GameObject cardGo = createCard(i, player, parent, position);
                generatedCards.Add(cardGo);
                position++;
            }
            return generatedCards;
        }

        public CardInfo drawCard(Player player)
        {
            GameObject card = null;
            do
            {
                int index = UnityEngine.Random.Range(0, Sprites.Length);
                if (!taken.Contains(index))
                {
                    taken.Add(index);
                    UnPlayer unPlayer = UnPlayer.getUnPlayer(player);
                    return new CardInfo(unPlayer, unPlayer.getDeck().Count, index);
                }
            } while (card == null);
            return null;
        }


        public GameObject createCard(int index, Player owner, GameObject parent, int position)
        {
            GameObject cardGo = Instantiate(cardPrefab, parent.transform);
            cardGo.GetComponent<SpriteRenderer>().sprite = Sprites[index];
            Vector3 v3 = cardGo.GetComponent<Transform>().position;
            Card card = cardGo.GetComponent<Card>();
            card.setOwner(UnPlayer.getUnPlayer(owner));
            card.setPosition(position);
            int? number;
            string color;
            bool isWild, isReverse, isSkip;
            int plusCards;
            getCardValues(Sprites[index], out color, out number, out isWild, out isReverse, out isSkip, out plusCards);
            card.setColor(color);
            if (number != null)
                card.setNumber((int)number);
            else
                card.setNumber(-1);
            if(isWild)
                card.isWild(isWild);
            if(isReverse)
                card.isReverse(isReverse);
            if(isSkip)
                card.isSkip(isSkip);
            card.setPlusCards(plusCards);
            Vector3 localPosition = cardGo.GetComponent<Transform>().localPosition;
            Vector3 newV3 = new Vector3(localPosition.x, localPosition.y, -position);
            cardGo.GetComponent<Transform>().localPosition = newV3;
            return cardGo;
        }

        public static void getCardValues(Sprite sprite, out string color, out int? number, out bool isWild, out bool isReverse, out bool isSkip, out int plusCards)
        {
            plusCards = 0;
            string name = sprite.name;
            bool wild = false, reverse = false, skip = false;
            switch (name[0])
            {
                case 'r':
                    color = "red";
                    break;
                case 'g':
                    color = "green";
                    break;
                case 'b':
                    color = "blue";
                    break;
                case 'y':
                    color = "yellow";
                    break;
                case 'w':
                    {
                        if (name[1] == 'c')
                            wild = true;
                        color = "";
                        break;
                    }
                default:
                    color = "";
                    break;
            }
            try
            {
                number = int.Parse(name[1].ToString());
            }
            catch (FormatException)
            {
                number = null;
                switch (name[1])
                {
                    case 's':
                        skip = true;
                        break;
                    case 'p':
                        {
                            try
                            {
                                if (name.Length > 2)
                                {
                                    plusCards = int.Parse(name[2].ToString());
                                }
                                else
                                {
                                    plusCards = 2;
                                }
                            }
                            catch (FormatException)
                            {
                                break;
                            }
                            break;
                        }
                    case 'r':
                        reverse = true;
                        break;
                    default:
                        break;
                }
            }
            isReverse = reverse;
            isSkip = skip;
            isWild = wild;
        }
        #endregion

        #region Public Methods

        public void startGame()
        {
            Player[] players = PhotonNetwork.PlayerList;
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    sendGeneratedCards(ToObjArr(generateCardIndices()), players[i], RpcTarget.All, i);
                }
            }
        }

        public object[] ToObjArr(int[] indices)
        {
            List<object> objArr = new List<object>();
            foreach (int i in indices)
            {
                objArr.Add(i);
            }
            return objArr.ToArray();
        }

        public void sendGeneratedCards(object[] indices, Player player, RpcTarget target, int PlayerIndex)
        {
            photonView.RPC("receiveCards", target, player, indices, PlayerIndex);
        }

        public void onDrawCard()
        {
            if (turn != localPlayer)
                return;

            photonView.RPC("drawCard", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, localPlayer);
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
                UnPlayer unPlayer = UnPlayer.getUnPlayer(player);
                List<GameObject> cards = generateCards(player, localPlayerDeck, indexList.ToArray(), playerIndex, unPlayer.getDeck().Count);
                playerDeck = cards;
                unPlayer.addCards(CardInfo.getCards(cards));
                Players[playerIndex] = unPlayer;
            }
            else if (!player.Equals(PhotonNetwork.LocalPlayer))
            {
                if (remotePlayerInfo != null && remotePlayerRegion != null)
                {
                    UnPlayer unPlayer = UnPlayer.getUnPlayer(player);
                    if (unPlayer != null)
                    {
                        if (unPlayer.getRemotePlayerInfo() == null)
                        {
                            GameObject RemotePlayerInfo = Instantiate(remotePlayerInfo, remotePlayerRegion.transform);
                            unPlayer.setRemotePlayerInfo(RemotePlayerInfo);
                        }
                        unPlayer.addCards(CardInfo.generateCardList(indexList, unPlayer, unPlayer.getDeck().Count));
                        Players[playerIndex] = unPlayer;
                    }
                }
            }
        }

        [PunRPC]
        void drawCard(Player player, int playerIndex)
        {
            if (Players[localPlayer].getOwner().IsMasterClient)
            {
                if (turn == playerIndex)
                {
                    CardInfo ci = drawCard(player);
                    object[] index = { ci.getCardIndex() };
                    sendGeneratedCards(index, player, RpcTarget.All, playerIndex);
                }
            }
        }
        #endregion

    }
}
