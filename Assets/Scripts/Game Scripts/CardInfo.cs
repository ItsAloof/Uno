using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Un;
using UnityEngine;

public class CardInfo
{
    UnPlayer Owner { get; set; }
    int Position { get; set; }
    int CardIndex { get; set; }
    public CardInfo(UnPlayer owner, int position, int cardIndex)
    {
        this.Owner = owner;
        this.Position = position;
        this.CardIndex = cardIndex;
    }


    public static List<CardInfo> generateCardList(List<int> indices, UnPlayer owner, int startingPosition)
    {
        List<CardInfo> cards = new List<CardInfo>();
        int position = startingPosition;
        foreach (int index in indices)
        {
            cards.Add(new CardInfo(owner, position, index));
            position++;
        }
        return cards;
    }

    public static List<CardInfo> getCards(List<GameObject> cards)
    {
        List<CardInfo> scripts = new List<CardInfo>();
        foreach (GameObject go in cards)
        {
            Card cardScript = go.GetComponent<Card>();
            CardInfo ci = cardScript.toCardInfo();
            scripts.Add(ci);
        }
        return scripts;
    }

    public UnPlayer getOwner()
    {
        return Owner;
    }

    public int getPosition()
    {
        return Position;
    }

    public void setPosition(int position)
    {
        this.Position = position;
    }

    public int getCardIndex()
    {
        return CardIndex;
    }

    public GameObject instantiateToCard()
    {
        GameObject go = Card.instantiateCard(CardIndex, GameObject.FindGameObjectWithTag("Discard"));
        go.GetComponent<SpriteRenderer>().sprite = GameManager.gameManager.Sprites[CardIndex];
        return go;
    }
}
