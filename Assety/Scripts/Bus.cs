using UnityEngine;
using TMPro;

public class ReactiveSpriteMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Settings")]
    [SerializeField] private float triggerDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float escapeSpeed = 4f;
    [SerializeField] private float escapeOffset = 5f;

    private float startX;
    private bool escaped = false;
    private int escapeDirection = 0;

    private void Start()
    {
        startX = transform.position.x;
        UpdateText();
    }

    private void Update()
    {
        if (player == null) return;

        if (escaped)
        {
            // Escaped mode, keep moving in escapeDirection at escapeSpeed
            transform.position += new Vector3(escapeDirection * escapeSpeed * Time.deltaTime, 0f, 0f);
            return;
        }

        float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);

        if (distanceToPlayer < triggerDistance)
        {
            // Player is close, move away
            if (player.position.x > transform.position.x)
                Move(-1);
            else
                Move(1);

            CheckEscape();
        }
        else
        {
            // Player is far, return to startX if not already there
            if (Mathf.Abs(transform.position.x - startX) > 0.1f)
            {
                if (transform.position.x > startX)
                    Move(-1);
                else
                    Move(1);
            }
        }
    }

    void Move(int direction)
    {
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0f, 0f);
    }

    void CheckEscape()
    {
        if (transform.position.x <= startX - escapeOffset)
        {
            escaped = true;
            escapeDirection = -1;
            UpdateText();
        }
        else if (transform.position.x >= startX + escapeOffset)
        {
            escaped = true;
            escapeDirection = 1;
            UpdateText();
        }
    }

    void UpdateText()
    {
        if (infoText != null)
            infoText.text = escaped ? "Ujel ti autobus, musis jit pesky.  :(" : "Nastup do autobusu.";
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            startX = transform.position.x;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(startX, transform.position.y + 0.5f), new Vector3(startX, transform.position.y - 0.5f));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(startX + escapeOffset, transform.position.y + 0.5f), new Vector3(startX + escapeOffset, transform.position.y - 0.5f));
        Gizmos.DrawLine(new Vector3(startX - escapeOffset, transform.position.y + 0.5f), new Vector3(startX - escapeOffset, transform.position.y - 0.5f));
    }
}
