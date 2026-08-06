using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public Transform platformPrefab;
    public Transform coinPrefab;
    public Transform boostPrefab;
    public int platformsPerLevel = 20;
    public float platformLength = 4f;
    public Vector3 startPosition = Vector3.zero;
    public float baseGap = 1f;
    public float gapVariance = 0.5f;
    public float levelGapIncrease = 0.6f; // increases gap per level
    public float coinChance = 0.5f;
    public float boostChance = 0.08f;

    List<Transform> spawned = new List<Transform>();
    int currentLevel = 1;
    Vector3 spawnCursor;

    void Start() {
        spawnCursor = startPosition;
        GenerateLevel(currentLevel);
    }

    public void GenerateLevel(int levelNumber) {
        ClearLevel();
        float gapModifier = baseGap + (levelNumber - 1) * levelGapIncrease;
        for (int i = 0; i < platformsPerLevel; i++) {
            float gap = gapModifier + Random.Range(-gapVariance, gapVariance);
            Vector3 platPos = spawnCursor + new Vector3((platformLength + gap) * (i==0?0:1), 0, (platformLength + gap) * 1f * (spawned.Count==0 ? 0 : 1));
            // We'll place platforms in a line along Z for simplicity. You can randomize x for side jumps.
            Transform p = Instantiate(platformPrefab, spawnCursor + new Vector3(0, 0, (platformLength + gap) * spawned.Count), Quaternion.identity, transform);
            spawned.Add(p);
            // spawn coins
            if (Random.value < coinChance) {
                Vector3 coinPos = p.position + Vector3.up * 1.2f + Vector3.right * Random.Range(-platformLength/2 + 0.3f, platformLength/2 - 0.3f);
                Instantiate(coinPrefab, coinPos, Quaternion.identity, transform);
            }
            // spawn occasional boosts
            if (Random.value < boostChance) {
                Vector3 bPos = p.position + Vector3.up * 0.5f + Vector3.right * Random.Range(-platformLength/2 + 0.3f, platformLength/2 - 0.3f);
                Instantiate(boostPrefab, bPos, Quaternion.identity, transform);
            }
        }
        // place finish platform
        Transform finish = Instantiate(platformPrefab, spawnCursor + Vector3.forward * (platformLength + gapModifier) * platformsPerLevel + Vector3.forward * 4f, Quaternion.identity, transform);
        // TODO: mark finish with a different visual and a trigger
    }

    public void ClearLevel() {
        foreach (var t in spawned) if (t) Destroy(t.gameObject);
        foreach (Transform child in transform) {
            // remove children (coins/boosts/platforms). We'll keep references short.
            if (child != null) Destroy(child.gameObject);
        }
        spawned.Clear();
    }

    public void NextLevel() {
        currentLevel++;
        GenerateLevel(currentLevel);
        FindObjectOfType<GameManager>().SetLevel(currentLevel);
    }
}