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
    string Color { get; set; }
    int Number { get; set; }
    bool IsWild { get; set; } = false;
    bool IsReverse { get; set; } = false;
    bool IsSkip { get; set; } = false;
    int PlusCards { get; set; } = 0;
    public CardInfo(UnPlayer owner, int position, int cardIndex)
    {
        this.Owner = owner;
        this.Position = position;
        this.CardIndex = cardIndex;
    }
    public CardInfo(UnPlayer owner, int position, int cardIndex, string color, int number)
    {
        this.Owner = owner;
        this.Position = position;
        this.CardIndex = cardIndex;
        this.Color = color;
        this.Number = number;
    }


    public static List<CardInfo> generateCardList(List<int> indices, UnPlayer owner, int startingPosition)
    {
        List<CardInfo> cards = new List<CardInfo>();
        int position = startingPosition;
        foreach (int index in indices)
        {
            GameManager gameManager = GameManager.gameManager;
            int? numOut;
            int number = -1, plusCards;
            bool isWild, isReverse, isSkip;
            string color;
            GameManager.getCardValues(gameManager.Sprites[index], out color, out numOut, out isWild, out isReverse, out isSkip, out plusCards);
            if(numOut != null)
            {
                number = (int)numOut;
            }
            CardInfo ci = new CardInfo(owner, position, index, color, number);
            if (isWild)
                ci.isWild(isWild);
            if (isReverse)
                ci.isReverse(isReverse);
            if (isSkip)
                ci.isSkip(isSkip);
            ci.setPlusCards(plusCards);
            cards.Add(ci);
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

    public void setColor(string color)
    {
        this.Color = color;
    }

    public string getColor()
    {
        return this.Color;
    }

    public bool canPlay(CardInfo ci)
    {
        if(ci.Color == this.Color || (ci.Number == this.Number && ci.Number >= 0))
        {
            return true;
        }

        return false;
    }

    public GameObject instantiateToCard()
    {
        GameObject go = Card.instantiateCard(GameObject.FindGameObjectWithTag("Discard"));
        go.GetComponent<SpriteRenderer>().sprite = GameManager.gameManager.Sprites[CardIndex];
        setCard(go);
        Card card = getCardScript();
        card.setPosition(Position);
        card.setOwner(Owner);
        card.setCardIndex(CardIndex);
        card.setCardInfo(this);
        card.setColor(Color);
        card.setNumber(Number);
        card.isWild(IsWild);
        card.isReverse(IsReverse);
        card.isSkip(IsSkip);
        card.setPlusCards(PlusCards);
        return go;
    }

    public void discard()
    {
        Owner.getDeck().RemoveAt(Position);
        Owner.updateCardPositions();
    }
}
