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
                GameObject go = ci.instantiateToCard();
                //ci.getOwner().removeCard(ci.getPosition());
                GameManager gameManager = GameManager.gameManager;
                Vector3 localPosition = go.GetComponent<Transform>().localPosition;
                Vector3 newV3 = new Vector3(localPosition.x, localPosition.y, -Card.cards);
                go.GetComponent<Transform>().localPosition = newV3;
                gameManager.discardPile.Add(go);
                gameManager.turn = turnDatum;
            }
        }

        public CardInfo findCard(int position, int ownerId)
        {
            List<UnPlayer> players = GameManager.gameManager.Players;
            for (int i = 0; i <= players.Count; i++)
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