using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Disable the wall when the ball collides with it
            gameObject.SetActive(false);
        }
    }

    public void ResetWall()
    {
        // Re-enable the wall when needed (e.g., at the start of a new round)
        gameObject.SetActive(true);
    }
}
