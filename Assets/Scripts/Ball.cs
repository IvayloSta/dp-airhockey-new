using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoToolkit;

public class Ball : MonoBehaviour {

    public float startingSpeed = 4f; //3f
    public float maxSpeed = 7f; //5f
    public bool easy = false;
    private PlayerSoundEffect soundEffects;
    private float speed;
    private Vector3 initPosition = new Vector3(0, 0, -6f);
    private Vector3 direction;
    private bool isOutOfBounds = false;
    private Rigidbody rb;

    private float hitCooldown = 0f;
    private float hitCooldownDuration = 0.1f; // 100ms delay


    void Start() {
        soundEffects = GetComponent<PlayerSoundEffect>();
        rb = GetComponent<Rigidbody>();
        Reset();
    }

    /* void FixedUpdate()
    {
        if (rb.velocity.magnitude < 0.01f && !isOutOfBounds)
        {
            rb.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
            //rb.velocity = direction.normalized * speed;
        }
        
    }
*/
    void Update()
    {
        if (isOutOfBounds)
        {
            isOutOfBounds = false;
            Reset();
        }
       
        if (hitCooldown > 0f)
            hitCooldown -= Time.deltaTime;
    }

    void Reset()
    {
        transform.position = initPosition;
        int initAngle = 0;
        if (easy)
        {
            initAngle = UnityEngine.Random.Range(-20, 20);
        }
        else
        {
            int initAngleLeft = UnityEngine.Random.Range(-65, -25);
            int initAngleRight = UnityEngine.Random.Range(25, 65);
            initAngle = UnityEngine.Random.Range(0, 2) == 0 ? initAngleLeft : initAngleRight;
        }
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

    void OnCollisionEnter(Collision other)
    {
        //prevents double bounces:
        if (hitCooldown > 0f) return;
        hitCooldown = hitCooldownDuration;


        //Vector3 reflection = ComputeReflection(other);
        //not custom just use:
        ContactPoint contact = other.GetContact(0);
        Vector3 reflection = Vector3.Reflect(rb.velocity.normalized, contact.normal);

        if (other.collider.CompareTag("Player"))
        {
            soundEffects.PlayPaddleClip();
            Player player = other.collider.GetComponent<Player>();
            Vector3 paddleVelocity = player != null ? player.velocity : Vector3.zero;

            float paddleSpeed = paddleVelocity.magnitude;
            float incomingSpeed = rb.velocity.magnitude;

            Vector3 finalVelocity;

            if (paddleSpeed < 0.2f) //paddle almost stationary
            {
                // Stationary hit: reflect at same or reduced speed
                float dampFactor = 0.6f; // reduce slightly
                finalVelocity = reflection.normalized * (incomingSpeed * dampFactor);
            }
            else
            {
                // Moving hit: apply current boost logic
                Vector3 boostA = reflection.normalized * speed * Mathf.Max(0.1f, Vector3.Dot(reflection.normalized, paddleVelocity.normalized) + 1f);
                Vector3 boostB = reflection.normalized * speed + paddleVelocity * 0.7f;
                finalVelocity = boostA.magnitude > boostB.magnitude ? boostA : boostB;
            }

            // Clamp speed and apply
            finalVelocity = Vector3.ClampMagnitude(finalVelocity, maxSpeed + 2f);
            
            speed = Mathf.Min(finalVelocity.magnitude, maxSpeed + 2f);
            rb.velocity = finalVelocity;
        }
        else if (other.collider.CompareTag("Wall"))
        {
            soundEffects.PlayWallClip();

            rb.velocity = reflection.normalized * speed;
            //this.direction = reflection;

        }
        else if (other.collider.CompareTag("PlayerScoreLine"))
        {
            soundEffects.PlayScoreClip();
            isOutOfBounds = true;
        }
        else if (other.collider.CompareTag("EnemyScoreLine"))
        {
            soundEffects.PlayPositiveScoreClip();
            isOutOfBounds = true;
        }

        
        this.direction = rb.velocity.normalized;
        
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
