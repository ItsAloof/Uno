using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Tooltip("Card Prefab")]
    [SerializeField]
    GameObject cardPrefab;

    [Tooltip("List of all card images")]
    [SerializeField]
    public Sprite[] Sprites;

    [SerializeField]
    GameObject localPlayer;

    Ray ray;
    RaycastHit hit;


    List<GameObject> cards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
        generateCards();
    }

    void generateCards()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject cardGo = Instantiate(cardPrefab, localPlayer.transform);
            cardGo.GetComponent<SpriteRenderer>().sprite = Sprites[i];
            Vector3 v3 = cardGo.GetComponent<Transform>().position;
            Vector3 newV3 = new Vector3(-200 + i*50, v3.y + 100, 0 - i*2);
            Debug.Log($"Old V3: x = {v3.x}, y = {v3.y}, z = {v3.z}\nNew V3: x = {newV3.x}, y = {newV3.y}, z = {newV3.z}");
            cardGo.GetComponent<Transform>().localPosition = newV3;
            Debug.Log($"{cardGo.GetComponent<Transform>().localPosition}");
            cards.Add(cardGo);
        }
    }

    public 

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log($"{hit.collider.gameObject.name}");
            if (cards.Contains(hit.collider.gameObject))
            {
                Debug.Log($"{hit.collider.gameObject.name}");
            }
        }
    }
}
