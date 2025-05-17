using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player3 : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_jumpAction;
    private InputAction m_shootAction;

    private Vector2 moveDir;
    private Vector2 deathPoint;
    private bool canDoubleJump;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool canShoot = true;
    private bool isDead = false;
    private int shotsFired = 0;
    private AudioSource sfx;

    private Vector2 lastPos;
    private Vector2 moveDis;

    private Rigidbody2D rb;
    private Animator animator;

    [Header("Editor")]
    [SerializeField] private bool isGod = false;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float dJumpForce = 12f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground")]
    [SerializeField] LayerMask groundMask;

    [Header("Audio")]
    [SerializeField] AudioClip jumpSfx;
    [SerializeField] AudioClip dJumpSfx;
    [SerializeField] AudioClip shootSfx;
    [SerializeField] AudioClip deathSfx;

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    [SerializeField] private float gunCooldown;

    [SerializeField] private float shootCooldown = 0.25f;
    [SerializeField] private float reloadCooldown = 1.2f;
    [SerializeField] private int maxShots = 4;

    [Header("Death")]
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject DeathEffectPrefab;

    // Input System
    private void OnEnable() {
        InputActions.FindActionMap("Player").Enable();

        m_jumpAction.performed += OnJump;
        m_shootAction.performed += OnShoot;
    }
    private void OnDisable() {
        InputActions.FindActionMap("Player").Disable();

        m_jumpAction.performed -= OnJump;
        m_shootAction.performed -= OnShoot;
    }

    private void Awake() {
        var playerMap = InputActions.FindActionMap("Player");
        m_moveAction = playerMap.FindAction("Move");
        m_jumpAction = playerMap.FindAction("Jump");
        m_shootAction = playerMap.FindAction("Shoot");

        m_jumpAction.performed += OnJump;
        m_shootAction.performed += OnShoot;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sfx = GetComponent<AudioSource>();

        lastPos = transform.position;
        GameOverScreen.SetActive(false);
    }

    void Update() {
        GroundCheck();
    }

    void FixedUpdate() {
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("onGround", isGrounded);
        OnMove();
        Animate();
    }

    private void GroundCheck() {
        isGrounded = Physics2D.Raycast((Vector2)transform.position + Vector2.left * 0.2f, Vector2.down, 0.6f, groundMask) ||
                           Physics2D.Raycast((Vector2)transform.position + Vector2.right * 0.2f, Vector2.down, 0.6f, groundMask);
    }

    private void OnMove() {
        moveDir = m_moveAction.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDir.x * moveSpeed, rb.velocity.y);
        if (moveDir.x != 0) {
            if (moveDir.x < 0 && isFacingRight || moveDir.x > 0 && !isFacingRight) {
                isFacingRight = !isFacingRight;
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        animator.SetBool("isJumping", false);
        if (canDoubleJump && !isGrounded)
        {
            animator.SetBool("isJumping", true);
            rb.velocity = Vector2.zero;
            sfx.PlayOneShot(dJumpSfx);

            rb.AddForce(Vector2.up * dJumpForce, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
        if (isGrounded)
        {
            animator.SetTrigger("preJump");
            animator.SetBool("isJumping", true);
            sfx.PlayOneShot(jumpSfx);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = true;
        }
    }

    private void OnShoot(InputAction.CallbackContext context) {
        if (!canShoot) return;

        sfx.PlayOneShot(shootSfx);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        shotsFired++;

        if (shotsFired >= maxShots)
        {
            StartCoroutine(ReloadCooldown());
        }
        else
        {
            StartCoroutine(ShotCooldown());
        }
    }

    private IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private IEnumerator ReloadCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(reloadCooldown);
        shotsFired = 0;
        canShoot = true;
    }

    private void Animate() {
        Vector2 curPos = transform.position;
        moveDis = (curPos - lastPos);

        if (Mathf.Abs(moveDis.x) > 0.001f && m_moveAction.ReadValue<Vector2>() != Vector2.zero) {
            animator.SetBool("isWalking", true);
        }
        else {
            animator.SetBool("isWalking", false);
        }

        lastPos = curPos;
    }

    public void TakeDamage(int damagePoints)
    {
        if (!isDead && !isGod) {
            isDead = true;
            //deathPoint = transform.position; 
            //sfx.PlayOneShot(deathSfx);
            //StartCoroutine(BurstSequence());
            Instantiate(DeathEffectPrefab, Vector3.zero, Quaternion.identity);
            GameOverScreen.SetActive(true);
            Destroy(this.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay((Vector2)this.transform.position + Vector2.left * 0.2f, Vector2.down * 0.6f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay((Vector2)this.transform.position + Vector2.right * 0.2f, Vector2.down * 0.6f);
    }
#endif
}
