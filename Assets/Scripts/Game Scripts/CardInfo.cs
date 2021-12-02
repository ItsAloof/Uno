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
    GameObject card { get; set; }
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
            cardScript.setCardInfo(ci);
            scripts.Add(ci);
        }
        return scripts;
    }

    public void setCard(GameObject card)
    {
        this.card = card;
    }

    public GameObject getCard()
    {
        return card;
    }

    public Card getCardScript()
    {
        if (card == null)
            return null;
        return card.GetComponent<Card>();
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
        //Debug.Log($"CardInfo.instantiateToCard: Position = {Position}");
        GameObject go = Card.instantiateCard(GameObject.FindGameObjectWithTag("Discard"));
        go.GetComponent<SpriteRenderer>().sprite = GameManager.gameManager.Sprites[CardIndex];
        setCard(go);
        Card card = getCardScript();
        card.setPosition(Position);
        card.setOwner(Owner);
        card.setCardIndex(CardIndex);
        card.setCardInfo(this);
        return go;
    }

    public void discard()
    {
        //Debug.Log($"CardInfo.discard(): {Position}");
        Owner.getDeck().RemoveAt(Position);
        Owner.updateCardPositions();
    }
}
