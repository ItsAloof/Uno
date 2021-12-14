using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Un
{
    public class CardManager : MonoBehaviourPunCallbacks
    {
        int TurnData = 0, PositionData = 1, PlayerIdData = 2, DirectionData = 3;
        [SerializeField]
        public static int CurrentPlusCards = 0;
        [SerializeField]
        public static int CurrentPlusType = 0;
        public AudioSource cardSound;

        public override void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_TurnEvent;
        }

        public override void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_TurnEvent;
        }

        private void NetworkingClient_EventReceived(EventData obj)
        {
            if (obj.Code == EventCodes.MOVE_CARD_EVENT)
            {
                object[] data = (object[])obj.CustomData;
                int positionDatum = (int)data[PositionData];
                int turnDatum = (int)data[TurnData];
                int playerIdDatum = (int)data[PlayerIdData];
                int directionDatum = (int)data[DirectionData];

                CardInfo ci = findCard(positionDatum, playerIdDatum);
                if (ci == null)
                    return;
                cardSound.Play();
                GameManager gameManager = GameManager.gameManager;
                GameObject card = ci.getCard();
                if (card == null)
                {
                    card = ci.instantiateToCard();
                }
                card.GetComponent<Card>().discard();
                GameObject remoteInfo = ci.getOwner().getRemotePlayerInfo();
                if (remoteInfo != null)
                    remoteInfo.GetComponent<PlayerInfo>().setCardCount(ci.getOwner().getDeck().Count);
                gameManager.turn = turnDatum;
                gameManager.direction = directionDatum;
                updateTurnIndicator();
                if(PhotonNetwork.IsMasterClient && ci.getPlusCards() > 0 && CurrentPlusCards == 0 && CurrentPlusType == 0)
                {
                    CurrentPlusType = ci.getPlusCards();
                    CurrentPlusCards = ci.getPlusCards();
                }
                else if (CardManager.CurrentPlusType == ci.getPlusCards())
                    CardManager.CurrentPlusCards += ci.getPlusCards();
                if (PhotonNetwork.IsMasterClient && ci.getPlusCards() > 0 && ci.getPlusCards() == CurrentPlusCards)
                {
                    Debug.Log($"Current turn {turnDatum} and person having plus card used against them is {gameManager.Players[turnDatum]}");
                    if(ci.isWild())
                    {
                        onPlusCards(gameManager, gameManager.Players[getNextTurn(turnDatum, directionDatum)].getOwner(), getNextTurn(turnDatum, directionDatum));
                    }
                    else
                    {
                        onPlusCards(gameManager, gameManager.Players[turnDatum].getOwner(), turnDatum);
                    }
                }
                else if (PhotonNetwork.IsMasterClient)
                {
                    CurrentPlusType = 0;
                    CurrentPlusCards = 0;
                }
            }
        }

        public static void onPlusCards(GameManager gameManager, Player player, int turn)
        {
            Debug.Log("Running onPlusCards...");
            if (gameManager.giveCards(player, UnPlayer.getUnPlayer(player).getOwnerId(), CurrentPlusCards, CurrentPlusType, turn))
            {
                Debug.Log("Giving cards to player");
                gameManager.turn = getNextTurn(gameManager.turn, gameManager.direction);
                object[] turnData = new object[] { gameManager.turn };
                PhotonNetwork.RaiseEvent(EventCodes.END_TURN_EVENT, turnData, RaiseEventOptions.Default, SendOptions.SendReliable);
            }
        }

        public void NetworkingClient_TurnEvent(EventData obj)
        {
            Debug.Log("Skipping player.");
            if (obj.Code == EventCodes.END_TURN_EVENT)
            {
                object[] data = (object[])obj.CustomData;
                int turnDatum = (int)data[0];
                GameManager.gameManager.turn = turnDatum;
                updateTurnIndicator();
            }
        }

        public static int getNextTurn(int turn, int direction)
        {
            int next = turn + direction;
            if (next >= GameManager.gameManager.Players.Count)
                next = 0;
            else if (next < 0)
                next = GameManager.gameManager.Players.Count - 1;
            return next;
        }

        public static void updateTurnIndicator()
        {
            List<UnPlayer> players = GameManager.gameManager.Players;
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].getOwner().IsLocal)
                {
                    UnPlayer player = players[i];
                    if (player.getRemotePlayerInfo() == null)
                        return;
                    PlayerInfo pi = player.getRemotePlayerInfo().GetComponent<PlayerInfo>();
                    if (pi.isNotTurnIndicatorActive() && GameManager.gameManager.turn == player.getOwnerId())
                    {
                        pi.toggleTurnIndicator();
                    }
                    else if (pi.isTurnIndicatorActive() && GameManager.gameManager.turn != player.getOwnerId())
                    {
                        pi.toggleTurnIndicator();
                    }
                }
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