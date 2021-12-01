using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Un;
using UnityEngine;

public class UnPlayer
{
    private List<CardInfo> deck { get; set; }

    private Player owner { get; set; }
    private int ownerId { get; set; }
    private GameObject RemotePlayerInfo;
    List<GameObject> cards { get; set; }

    public UnPlayer(Player owner, int ownerId)
    {
        this.owner = owner;
        this.ownerId = ownerId;
        deck = new List<CardInfo>();
    }
    public UnPlayer(Player owner, int ownerId, GameObject remotePlayerInfo)
    {
        this.owner = owner;
        this.ownerId = ownerId;
        this.RemotePlayerInfo = remotePlayerInfo;
        deck = new List<CardInfo>();
    }

    public UnPlayer(Player owner, int ownerId, List<CardInfo> deck)
    {
        this.owner = owner;
        this.ownerId = ownerId;
        deck = new List<CardInfo>();
        deck.AddRange(deck);
    }


    public UnPlayer(Player owner, int ownerId, List<CardInfo> deck, GameObject remotePlayerInfo)
    {
        this.owner = owner;
        this.ownerId = ownerId;
        deck = new List<CardInfo>();
        deck.AddRange(deck);
        this.RemotePlayerInfo = remotePlayerInfo;
    }

    public static UnPlayer getUnPlayer(Player player)
    {
        foreach(UnPlayer unPlayer in GameManager.gameManager.Players)
        {
            if (unPlayer.getOwner() == player)
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
        foreach(int index in indices)
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

    public void removeCard(int position)
    {
        deck.RemoveAt(position);
        updateCardPositions();
    }

    public void updateCardPositions()
    {
        for(int i = 0; i < deck.Count; i++)
        {
            deck[i].setPosition(i);
        }
    }

    public Player getOwner()
    {
        return owner;
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
}
