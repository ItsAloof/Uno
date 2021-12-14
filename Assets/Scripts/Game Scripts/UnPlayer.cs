using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Un
{
    public class UnPlayer
    {
        private List<CardInfo> deck { get; set; }

        private Player Player { get; set; }
        private int ownerId { get; set; }
        private GameObject RemotePlayerInfo;
        List<GameObject> cards { get; set; }

        public UnPlayer(Player player, int ownerId)
        {
            this.Player = player;
            this.ownerId = ownerId;
            deck = new List<CardInfo>();
        }
        public UnPlayer(Player player, int ownerId, GameObject remotePlayerInfo)
        {
            this.Player = player;
            this.ownerId = ownerId;
            this.RemotePlayerInfo = remotePlayerInfo;
            deck = new List<CardInfo>();
        }

        public UnPlayer(Player player, int ownerId, List<CardInfo> deck)
        {
            this.Player = player;
            this.ownerId = ownerId;
            deck = new List<CardInfo>();
            deck.AddRange(deck);
        }


        public UnPlayer(Player player, int ownerId, List<CardInfo> deck, GameObject remotePlayerInfo)
        {
            this.Player = player;
            this.ownerId = ownerId;
            deck = new List<CardInfo>();
            deck.AddRange(deck);
            this.RemotePlayerInfo = remotePlayerInfo;
        }

        public static UnPlayer getUnPlayer(Player player)
        {
            foreach (UnPlayer unPlayer in GameManager.gameManager.Players)
            {
                if (unPlayer.getPlayer() == player)
                    return unPlayer;
            }
            return null;
        }

        public void addCards(int[] indices)
        {
            int position;
            if (deck is null)
                position = 0;
            else
                position = deck.Count;
            foreach (int index in indices)
            {
                deck.Add(new CardInfo(this, position, index));
                position++;
            }
        }

        public void addCards(List<CardInfo> cards)
        {
            deck.AddRange(cards);
        }

        public void addCard(int index)
        {
            deck.Add(new CardInfo(this, deck.Count, index));
        }

        public void updateCardPositions()
        {
            List<CardInfo> newDeck = new List<CardInfo>();
            for (int i = 0; i < deck.Count; i++)
            {
                CardInfo ci = deck[i];
                ci.setPosition(i);
                deck[i] = ci;
            }
        }

        public void updateCardColor(string color, int position)
        {
            CardInfo ci = deck[position];
            if(ci.getPosition() == position)
            {
                ci.setColor(color);
                deck[position] = ci;
            }
            
        }

        public void setRemotePlayerInfo(GameObject remotePlayerInfo)
        {
            this.RemotePlayerInfo = remotePlayerInfo;
        }

        public Player getPlayer()
        {
            return Player;
        }

        public int getOwnerId()
        {
            return ownerId;
        }

        public List<CardInfo> getDeck()
        {
            return deck;
        }

        public GameObject getRemotePlayerInfo()
        {
            return RemotePlayerInfo;
        }

        public CardInfo getCard(int position)
        {
            return deck[position];
        }
    }
}
