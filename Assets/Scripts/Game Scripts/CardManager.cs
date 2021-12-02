using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Un
{
    public class CardManager : MonoBehaviourPunCallbacks
    {
        int TurnData = 0, PositionData = 1, PlayerIdData = 2;
        public AudioSource cardSound;

        public override void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        }

        public override void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
        }

        private void NetworkingClient_EventReceived(EventData obj)
        {
            if (obj.Code == EventCodes.MOVE_CARD_EVENT)
            {

                object[] data = (object[])obj.CustomData;
                int positionDatum = (int)data[PositionData];
                int turnDatum = (int)data[TurnData];
                int playerIdDatum = (int)data[PlayerIdData];
                CardInfo ci = findCard(positionDatum, playerIdDatum);
                if (ci == null)
                    return;
                cardSound.Play();
                GameManager gameManager = GameManager.gameManager;
                GameObject card = ci.getCard();
                if(card == null)
                {
                    card = ci.instantiateToCard();
                }
                card.GetComponent<Card>().discard();
                GameObject remoteInfo = ci.getOwner().getRemotePlayerInfo();
                if (remoteInfo != null)
                    remoteInfo.GetComponent<PlayerInfo>().setCardCount(ci.getOwner().getDeck().Count);
                gameManager.turn = turnDatum;
            }
        }

        
        public static void updateCardPositions(UnPlayer owner)
        {
            Transform transform = GameManager.gameManager.localPlayerDeck.GetComponent<Transform>();
            for (int i = 0; i < owner.getDeck().Count; i++)
            {
                if (owner.getOwnerId() == GameManager.gameManager.localPlayer)
                {
                    if (i > transform.childCount)
                        return;
                    GameObject child = transform.GetChild(i).gameObject;
                    if (child != null)
                    {
                        child.GetComponent<Card>().getCardInfo().setPosition(i);
                        child.GetComponent<Card>().setPosition(i);
                        Vector3 v3 = new Vector3(0, 0, -i);
                        child.transform.localPosition = v3;
                    }
                }
            }
        }

        public CardInfo findCard(int position, int ownerId)
        {
            List<UnPlayer> players = GameManager.gameManager.Players;
            for (int i = 0; i < players.Count; i++)
            {
                foreach (CardInfo ci in players[i].getDeck())
                {
                    if (ci.getPosition() == position && ci.getOwner().getOwnerId() == ownerId)
                    {
                        return ci;
                    }
                }
            }
            return null;
        }

    }
}