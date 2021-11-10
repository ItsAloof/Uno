using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public static int cards = 0;
    int focusZPosition = -100;
    float oldZPosition;
    bool isDiscarded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseEnter()
    {
        if (isDiscarded)
            return;
        Vector3 v = transform.localPosition;
        oldZPosition = v.z;
        this.transform.localPosition = new Vector3(v.x, v.y, focusZPosition);
    }

    private void OnMouseExit()
    {
        if (isDiscarded)
            return;
        Vector3 v = transform.localPosition;
        this.transform.localPosition = new Vector3(v.x, v.y, oldZPosition);
    }

    private void OnMouseUpAsButton()
    {
        if (isDiscarded)
            return;
        GameObject go = GameObject.FindGameObjectWithTag("Discard");
        Vector3 v = new Vector3(0, 0, -cards);
        this.transform.SetParent(go.transform, false);
        isDiscarded = true;
        this.transform.localPosition = v;
        cards++;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
