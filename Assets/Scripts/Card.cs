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
        Player owner { get; set; }
        int ownerId { get; set; }
        Transform parent;
        GameManager gameManager;
        private int position;
        int TurnData = 0, PositionData = 1, PlayerIdData = 2;

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameManager.gameManager;
        }

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
                if (isDiscarded)
                    return;
                object[] data = (object[])obj.CustomData;
                int positionDatum = (int)data[PositionData];
                int turnDatum = (int)data[TurnData];
                int playerIdDatum = (int)data[PlayerIdData];

                Debug.Log($"PlayerId: {ownerId} PlayerIdDatum: {playerIdDatum} Position: {position} PositionData: {positionDatum} Turn: {gameManager.turn} TurnData: {turnDatum}");
                if (ownerId == playerIdDatum && position == positionDatum)
                {
                    discard(GameObject.FindGameObjectWithTag("Discard").transform);
                    gameManager.turn = turnDatum;
                }
            }
        }

        private void OnMouseEnter()
        {
            if (isDiscarded || PhotonNetwork.LocalPlayer != owner)
                return;
            Vector3 v = transform.localPosition;
            oldZPosition = v.z;
            this.transform.localPosition = new Vector3(v.x, v.y, focusZPosition);
        }

        private void OnMouseExit()
        {
            if (isDiscarded || PhotonNetwork.LocalPlayer != owner)
                return;
            Vector3 v = transform.localPosition;
            this.transform.localPosition = new Vector3(v.x, v.y, oldZPosition);
        }

        private void OnMouseUpAsButton()
        {
            if (isDiscarded || PhotonNetwork.LocalPlayer != owner || gameManager.turn != ownerId)
                return;
            GameObject go = GameObject.FindGameObjectWithTag("Discard");
            discard(go.transform);
            gameManager.turn++;
            if (gameManager.turn == gameManager.Players.Count)
            {
                gameManager.turn = 0;
            }
            object[] data = new object[] { gameManager.turn, position, ownerId };
            PhotonNetwork.RaiseEvent(EventCodes.MOVE_CARD_EVENT, data, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        private void discard(Transform parent)
        {
            Vector3 v = new Vector3(0, 0, -cards);
            this.transform.SetParent(parent, false);
            isDiscarded = true;
            this.transform.localPosition = v;
            cards++;
        }

        public void setOwner(Player player, int id)
        {
            owner = player;
            ownerId = id;
        }

        public void setPosition(int position)
        {
            this.position = position;
        }

        // Update is called once per frame
        void Update()
        {
        }

    }

    public class EventCodes
    {
        public static byte MOVE_CARD_EVENT = 1;
        public static byte DRAW_CARD = 2;
    }
}
