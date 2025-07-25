using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;

public class Recoil : MonoBehaviour
{
    PantoHandle handle;

    public string tag = "Ball";
    public bool isUpper = true;
    public bool handleFree = false;

    public float powerFactor = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        handle = isUpper
            ? (PantoHandle)GameObject.Find("Panto").GetComponent<UpperHandle>()
            : (PantoHandle)GameObject.Find("Panto").GetComponent<LowerHandle>();

    }

    /* Update is called once per frame
    //void Update()
    //{
    //    
    //}

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag(tag))
        {
            ContactPoint contact = other.contacts[0];
            Vector3 RecoilDirection = Vector3.Normalize(transform.position - contact.point);
            handle.MoveToPosition(transform.position + powerFactor * RecoilDirection, 10.0f, handleFree);
        }
    }*/
}
