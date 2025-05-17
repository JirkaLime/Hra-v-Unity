using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour
{
    [Header("Cloud Range")]
    [SerializeField] private Vector2 xRange = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 yRange = new Vector2(-2f, 2f);

    [Header("Cloud Speed")]
    [SerializeField] private float speed = 1;

    private void Start() {
        float startX = Random.Range(xRange.x, xRange.y);
        float startY = Random.Range(yRange.x, yRange.y);
        transform.position = new Vector3(startX, startY, transform.position.z);

        StartCoroutine(MoveCloud());
    }

    IEnumerator MoveCloud() {
        while (true) {
            Vector3 target = new Vector3(xRange.x, transform.position.y, transform.position.z);

            while (Vector3.Distance(transform.position, target) > 0.1f) {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
            float newY = Random.Range(yRange.x, yRange.y);
            transform.position = new Vector3(xRange.y, newY, transform.position.z);
        }
    }
}