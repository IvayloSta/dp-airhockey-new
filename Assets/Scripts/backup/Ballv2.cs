using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;

public class Ball_V2 : MonoBehaviour {

    public float startingSpeed = 3f;
    public float maxSpeed = 5f;
    private PlayerSoundEffect soundEffects;
    private float speed;
    private Vector3 initPosition = new Vector3(-1.15f, 0, -9.93f);
    private Vector3 direction;
    private bool isOutOfBounds = false;
    private Rigidbody rb;

    void Start() {
        soundEffects = GetComponent<PlayerSoundEffect>();
        rb = GetComponent<Rigidbody>();
        Reset();
    }

    void FixedUpdate() {
        if (rb.velocity.magnitude < 0.01f && !isOutOfBounds) {
            rb.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
        }
    }

    void Update() {
       if (isOutOfBounds) {
            isOutOfBounds = false;
            Reset();
       }
    }

    void Reset() {
        transform.position = initPosition;
        int initAngle = UnityEngine.Random.Range(-20, 20);
        direction = Quaternion.Euler(0, initAngle, 0) * new Vector3(0, 0, -1);
        speed = startingSpeed;
        rb.velocity = direction.normalized * speed;
    }

    Vector3 ComputeReflection(Collision other) {
        Vector3 normal = other.GetContact(0).normal.normalized;
        Vector3 collisionPoint = other.GetContact(0).point;

        float hitFactor = (collisionPoint.x - other.transform.position.x) / other.collider.bounds.size.x;
        hitFactor = Mathf.Clamp(hitFactor, -0.4f, 0.4f);
        hitFactor = normal.z < 0 ? hitFactor * (-1) : hitFactor;

        Vector3 reflection = Quaternion.Euler(0, hitFactor * 180, 0) * new Vector3(0, 0, normal.z);
        return reflection;
    }

    async void OnCollisionEnter(Collision other) {
        Vector3 reflection = ComputeReflection(other);

        if (other.collider.CompareTag("Player")) {
            soundEffects.PlayPaddleClip();
            speed = Mathf.Min(maxSpeed, speed + 0.1f);
        } else if (other.collider.CompareTag("Wall")) {
            soundEffects.PlayWallClip();
        } else if (other.collider.CompareTag("PlayerScoreLine")) {
            soundEffects.PlayScoreClip();
            isOutOfBounds = true;
        } else if (other.collider.CompareTag("EnemyScoreLine")) {
            soundEffects.PlayPositiveScoreClip();
            isOutOfBounds = true;
        }

        this.direction = reflection;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Speedboost")) {
            speed = Mathf.Min(maxSpeed, speed + 1f);
            Destroy(other.gameObject);
        }
    }

    public Vector3 GetDirection() {
        return this.direction;
    }
}
