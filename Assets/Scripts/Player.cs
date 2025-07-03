using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using DualPantoToolkit;

public class Player : MonoBehaviour
{
    private float speed = 2.0f;
    private PlayerSoundEffect soundEffects;
    private int score = 0;
    private Rigidbody rigidbody;
    PantoHandle handle;

    public bool isUpper = true;

    void Start()
    {
        handle = isUpper
            ? (PantoHandle)GameObject.Find("Panto").GetComponent<UpperHandle>()
            : (PantoHandle)GameObject.Find("Panto").GetComponent<LowerHandle>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ball"))
        {
            // Optional: add sound feedback here
            soundEffects?.PlayPaddleClip();
        }
    }

    private Vector3 lastPosition;
    public Vector3 velocity { get; private set; }

    void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
}
