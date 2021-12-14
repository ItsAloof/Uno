using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutManager : MonoBehaviour
{


    public GameObject playerPanel;
    public int cardcount;
    public GridLayoutGroup glg;
    

    // Update is called once per frame
    void Update()
    {
        cardcount = playerPanel.transform.childCount;
        if (cardcount <= 7)
        {
            Vector2 spacing = new Vector2(20, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 8)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(3, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 9)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-20, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 10)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-38, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 11)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-53, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 12)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-65, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 13)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-75, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 14)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-83, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 15)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-90, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 16)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-96, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 17)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-102, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 18)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-107, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 19)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-111, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 20)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-115, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 21)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-118, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 22)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-121, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 23)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-124, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 24)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-127, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 25)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-129, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 26)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-130, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 27)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-132, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 28)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-132, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 29)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-134, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount == 30)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-136, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else if (cardcount > 30 && cardcount <= 40)
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-150, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }
        else
        {
            GridLayoutGroup testing = playerPanel.GetComponent<GridLayoutGroup>();
            Vector2 spacing = new Vector2(-155, 0);
            glg = playerPanel.GetComponent<GridLayoutGroup>();
            glg.spacing = spacing;
        }

    }

    
}
