using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItem : MonoBehaviour
{

    public Text playerName;

    //Image backgroundImage;                 
    public Color highlightColor;
    //public GameObject leftArrowButton;      if we ever add user images
    //public GameObject rightArrowButton;     if we ever add user images

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
    }

    public void ApplyLocalChanges()
    {
        //backgroundImage.color = highlightColor;
        //leftArrowButton.setActive(true);
        //rightArrowButton.setActive(true);
    }

}
