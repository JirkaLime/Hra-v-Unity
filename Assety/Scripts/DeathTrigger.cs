using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] private SpriteBurst burstBlood;
    [SerializeField] private SpriteBurst burstParts; 
    [SerializeField] private int amount = 8;
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private Transform player;

    private AudioSource sfx;
    private Vector3 deathPoint;

    private void Awake()
    {
        sfx = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player").transform;
        deathPoint = player.position;
    }

    private void Start()
    {
        StartCoroutine(OnDead());
    }

    private IEnumerator OnDead()
    {
        sfx.PlayOneShot(deathSfx);

        burstParts.Burst(deathPoint);

        for (int i = 0; i < amount; i++) {
            burstBlood.Burst(deathPoint);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}