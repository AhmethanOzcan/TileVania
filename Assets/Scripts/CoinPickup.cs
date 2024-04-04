using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    bool isPicked = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !isPicked)
        {
            isPicked = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().increaseCoin();
            Destroy(gameObject);
        }
    }
}
