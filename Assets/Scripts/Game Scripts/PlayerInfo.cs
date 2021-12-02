using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    string Name;

    [SerializeField]
    int CardCount;

    [SerializeField]
    int OwnerId;

    [SerializeField]
    Text PlayerNamText;

    [SerializeField]
    Text CardCountText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOwnerId(int ownerId)
    {
        OwnerId = ownerId;
    }

    public int getOwnerId()
    {
        return OwnerId;
    }

    public void setCardCount(int count)
    {
        this.CardCount = count;
        CardCountText.text = $"{count}";
    }

    public int getCardCount()
    {
        return this.CardCount;
    }

    public void setName(string name)
    {
        Name = name;
        PlayerNamText.text = name;
    }
}
