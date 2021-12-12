using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutManager : MonoBehaviour
{


    public GameObject playerPanel;
    public int cardcount;
    public GridLayoutGroup glg;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cardcount = playerPanel.transform.childCount;
        if (cardcount <= 7)
        {
            Vector2 spacing = new Vector2(-25, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;


        }
        else if (cardcount > 7 && cardcount <= 14)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-75, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount > 14)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-110, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-130, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }

    }

    
}
