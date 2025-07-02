using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using DualPantoToolkit;

public class Player_old : MonoBehaviour
{


    private float speed = 2.0f;
    private PlayerSoundEffect soundEffects;
    private int score = 0;
    private Rigidbody rigidbody;
    PantoHandle handle;

    public bool isUpper = true;

    // Start is called before the first frame update
    void Start()
    {
        handle = isUpper
            ? (PantoHandle)GameObject.Find("Panto").GetComponent<UpperHandle>()
            : (PantoHandle)GameObject.Find("Panto").GetComponent<LowerHandle>();

    }
    

    async void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ball"))
        {
            ContactPoint contact = other.contacts[0];
            Vector3 RecoilDirection = Vector3.Normalize(transform.position - contact.point);
            Vector3 target = transform.position + 0.5f * RecoilDirection;
            if (!Physics.CheckSphere(target, 0.1f)) {
                await handle.MoveToPosition(target, 10.0f, true);
            }
        }
    }
}
