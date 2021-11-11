using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("Card Prefab")]
    [SerializeField]
    GameObject cardPrefab;

    [Tooltip("List of all card images")]
    [SerializeField]
    public Sprite[] Sprites;

    [Tooltip("The local players deck")]
    [SerializeField]
    GameObject localPlayer;



    static List<GameObject> cards = new List<GameObject>();
    List<int> taken = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        //Camera.main.depthTextureMode = DepthTextureMode.Depth;

        if (!PhotonNetwork.IsMasterClient)
        {
            generateCards();
        }
    }

    [PunRPC]
    void generateCards()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject cardGo = Instantiate(cardPrefab, localPlayer.transform);
        redo:
            int cI = Random.Range(0, Sprites.Length);
            if (taken.Contains(cI))
                goto redo;
            cardGo.GetComponent<SpriteRenderer>().sprite = Sprites[cI];
            Vector3 v3 = cardGo.GetComponent<Transform>().position;
            Vector3 newV3 = new Vector3(-300 + i * 100, v3.y + 150, 0 - i * 2);
            cardGo.GetComponent<Transform>().localPosition = newV3;
            Debug.Log($"{cardGo.GetComponent<Transform>().localPosition}");
            cards.Add(cardGo);
        }
        if(PhotonNetwork.pla)
    }



    // Update is called once per frame
    void Update()
    {
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if(Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log($"{hit.collider.gameObject.name}");
        //    if (cards.Contains(hit.collider.gameObject))
        //    {
        //        Debug.Log($"{hit.collider.gameObject.name}");
        //    }
        //}
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(cards);
            stream.SendNext(taken);
        }
        else
        {
            cards = (List<GameObject>)stream.ReceiveNext();
            taken = (List<int>)stream.ReceiveNext();
        }
    }
}
