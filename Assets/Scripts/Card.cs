using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    int focusZPosition = -100;
    float oldZPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseEnter()
    {
        Vector3 v = transform.localPosition;
        oldZPosition = v.z;
        this.transform.localPosition = new Vector3(v.x, v.y, focusZPosition);
    }

    private void OnMouseExit()
    {

        Vector3 v = transform.localPosition;
        this.transform.localPosition = new Vector3(v.x, v.y, oldZPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
