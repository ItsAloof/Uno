using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Un
{
    public class Card : MonoBehaviourPunCallbacks
    {
        public static int cards = 0;
        int focusZPosition = -100;
        float oldZPosition;
        bool isDiscarded = false;
        UnPlayer owner { get; set; }
        [SerializeField]
        string Color;
        //string Color { get; set; }
        [SerializeField]
        int Number;
        //int Number { get; set; }
        [SerializeField]
        bool IsWild = false;
        [SerializeField]
        bool IsReverse = false;
        [SerializeField]
        bool IsSkip = false;
        //bool IsWild { get; set; } = false;
        //bool IsReverse { get; set; } = false;
        //bool IsSkip { get; set; } = false;
        [SerializeField]
        int PlusCards = 0;

        Transform parent;
        GameManager gameManager;
        [SerializeField]
        public int position;

        

        private int CardIndex { get; set; }
        CardInfo cardInfo { get; set; }


        void Start()
        {
            gameManager = GameManager.gameManager;
        }

        private void OnMouseEnter()
        {
            if (isDiscarded || PhotonNetwork.LocalPlayer != owner.getOwner())
                return;
            Vector3 v = transform.localPosition;
            oldZPosition = v.z;
            this.transform.localPosition = new Vector3(v.x, v.y, focusZPosition);
        }

        private void OnMouseExit()
        {
            if (isDiscarded || PhotonNetwork.LocalPlayer != owner.getOwner())
                return;
            Vector3 v = transform.localPosition;
            this.transform.localPosition = new Vector3(v.x, v.y, oldZPosition);
        }

        private void OnMouseUpAsButton()
        {
            if (isDiscarded || PhotonNetwork.LocalPlayer != owner.getOwner() || gameManager.turn != owner.getOwnerId())
                return;

            if(canPlay())
            {
                CardInfo ci = cardInfo;
                if (IsReverse)
                    gameManager.direction = -gameManager.direction;
                if(IsSkip)
                {
                    gameManager.turn += gameManager.direction;
                    if (gameManager.turn >= gameManager.Players.Count)
                        gameManager.turn = 0;
                    if (gameManager.turn < 0)
                        gameManager.turn = gameManager.Players.Count-1;
                    gameManager.turn += gameManager.direction;
                }
                else if(!IsSkip)
                {
                    gameManager.turn += gameManager.direction;
                }
                if (gameManager.turn >= gameManager.Players.Count)
                {
                    gameManager.turn = 0;
                }
                if(gameManager.turn < 0)
                {
                    gameManager.turn = gameManager.Players.Count-1;
                }
                object[] data = new object[] { gameManager.turn, position, owner.getOwnerId(), gameManager.direction };
                discard();
                CardManager.updateTurnIndicator();
                GameManager.gameManager.cardSound.Play();
                PhotonNetwork.RaiseEvent(EventCodes.MOVE_CARD_EVENT, data, RaiseEventOptions.Default, SendOptions.SendReliable);
            }
        }

        public void discard()
        {
            Vector3 v = new Vector3(0, 0, -GameManager.gameManager.discardPile.Count);
            cardInfo.discard();
            this.transform.SetParent(GameObject.FindGameObjectWithTag("Discard").transform, false);
            isDiscarded = true;
            this.transform.localPosition = v;
            GameManager.gameManager.discardPile.Add(this.gameObject);
            CardManager.updateCardPositions(owner);
            cards++;
        }

        public bool canPlay()
        {
            if (gameManager.discardPile.Count == 0)
                return true;
            GameObject lastCard = gameManager.discardPile[gameManager.discardPile.Count-1];
            Card card = lastCard.GetComponent<Card>();
            if(card.getColor() == Color || card.getNumber() == Number || IsWild)
            {
                return true;
            }
            return false;
        }

        public void setOwner(UnPlayer owner)
        {
            this.owner = owner;
        }

        public void setColor(string color)
        {
            this.Color = color;
        }

        public string getColor()
        {
            return Color;
        }

        public void setNumber(int number)
        {
            this.Number = number;
        }

        public int getNumber()
        {
            return Number;
        }

        public void setPosition(int position)
        {
            this.position = position;
        }

        public UnPlayer getOwner()
        {
            return owner;
        }

        public void setCardInfo(CardInfo cardInfo)
        {
            this.cardInfo = cardInfo;
        }

        public CardInfo getCardInfo()
        {
            return cardInfo;
        }

        public int getCardIndex()
        {
            return CardIndex;
        }

        public CardInfo toCardInfo()
        {
            return new CardInfo(owner, position, CardIndex, Color, Number);
        }

        public static GameObject instantiateCard(GameObject parent)
        {
            return Instantiate(GameManager.gameManager.cardPrefab, parent.transform);
        }

        public void setCardIndex(int cardIndex)
        {
            this.CardIndex = cardIndex;
        }

        public void isWild(bool isWild)
        {
            IsWild = isWild;
        }
        public void isSkip(bool isSkip)
        {
            IsSkip = isSkip;
        }
        public void isReverse(bool isReverse)
        {
            IsReverse = isReverse;
        }

        public void setPlusCards(int plusCards)
        {
            this.PlusCards = plusCards;
        }

    }

    public class EventCodes
    {
        public static byte MOVE_CARD_EVENT = 1;
        public static byte DRAW_CARD = 2;
    }
}
