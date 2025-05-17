using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float speed = 20f;
    //[SerializeField] private float damage = 40f;

    private Rigidbody2D rb;


    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start() {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D (Collider2D hitInfo) {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
