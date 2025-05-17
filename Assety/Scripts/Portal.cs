using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [Header("Scene To Load")]
    [SerializeField] private string sceneName;
    [SerializeField] private bool returnPortal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (returnPortal) {
            Gizmos.color = Color.red;
        }
        else {
            Gizmos.color = Color.blue;
        }

        for (int i = 0; i < 5; i++) {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(0.3f, 0.3f, 0.3f) * i);
        }
    }
#endif
}
