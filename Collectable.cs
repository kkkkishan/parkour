using UnityEngine;

public enum CollectableType { Coin, Boost }

public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.Coin;
    public int coinValue = 1;
    public float boostMultiplier = 2f;
    public float boostDuration = 1.5f;
    public AudioClip pickupSound;

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        var gm = FindObjectOfType<GameManager>();
        if (type == CollectableType.Coin) {
            gm.AddCoins(coinValue);
        } else if (type == CollectableType.Boost) {
            var player = other.GetComponent<PlayerController>();
            if (player != null) player.ApplyBoost(boostMultiplier, boostDuration);
        }
        // optional sound
        if (pickupSound) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        Destroy(gameObject);
    }
}