using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int coins = 0;
    public int level = 1;
    public PlayerController player;
    public LevelManager levelManager;
    public Transform respawnPoint;
    public float fallY = -10f;
    public UIManager ui;

    void Start() {
        if (!player) player = FindObjectOfType<PlayerController>();
        if (!levelManager) levelManager = FindObjectOfType<LevelManager>();
        if (!ui) ui = FindObjectOfType<UIManager>();
        UpdateUI();
    }

    void Update() {
        if (player.transform.position.y < fallY) {
            Respawn();
        }
    }

    public void AddCoins(int n) {
        coins += n;
        UpdateUI();
        // simple rule: every 10 coins, increase level
        if (coins % 10 == 0) {
            NextLevel();
        }
    }

    public void NextLevel() {
        level++;
        levelManager.NextLevel();
        // optional: move player to start
        if (respawnPoint) player.transform.position = respawnPoint.position;
        UpdateUI();
    }

    public void SetLevel(int l) {
        level = l;
        UpdateUI();
    }

    public void Respawn() {
        if (respawnPoint) {
            player.transform.position = respawnPoint.position;
            var rb = player.GetComponent<Rigidbody>();
            if (rb) { rb.velocity = Vector3.zero; }
        } else {
            // fallback reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void UpdateUI() {
        if (ui) ui.UpdateUI(coins, level);
    }
}