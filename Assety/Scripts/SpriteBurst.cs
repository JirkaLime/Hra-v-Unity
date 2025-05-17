using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpriteBurst : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Prefab with a SpriteRenderer and Rigidbody2D on it")]
    [SerializeField] private GameObject particlePrefab;

    [Tooltip("All 81 sprites to choose from")]
    [SerializeField] private List<Sprite> allSprites;

    [Header("Burst Parameters")]
    [Tooltip("Center of the burst")]
    [SerializeField] private Transform burstOrigin;
    [Tooltip("How far from origin they can spawn")]
    [SerializeField] private float radius = 1f;

    public float burstForce = 5f;

    /// <summary>
    /// Call this to do one burst of up to allSprites.Count particles.
    /// </summary>
    public void Burst(Vector2 deathPoint)
    {
        if (allSprites == null || allSprites.Count == 0) return;

        // 1) Create a local copy of indices 0..N-1
        List<int> indices = Enumerable.Range(0, allSprites.Count).ToList();

        // 2) Shuffle them (Fisher�Yates)
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = indices[i];
            indices[i] = indices[j];
            indices[j] = tmp;
        }

        // 3) Instantiate one prefab per sprite, assign sprite, and apply force
        foreach (int idx in indices)
        {
            Vector2 pos = (Vector2)deathPoint + Random.insideUnitCircle * radius;

            GameObject go = Instantiate(particlePrefab, pos, Quaternion.identity);

            var sr = go.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = allSprites[idx];

            var rb = go.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1.2f)).normalized;
                rb.AddForce(randomDir * burstForce, ForceMode2D.Impulse);
            }
        }
    }
}
