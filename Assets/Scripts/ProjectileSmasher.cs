using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileSmasher : MonoBehaviour
{

    void Awake()
    {
        // Set position and width to be just above game view and full width of screen (no matter what size)
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        Camera cam = Camera.main;

        transform.position = new Vector3(0, cam.orthographicSize + transform.localScale.y/2f, 0);
        transform.localScale = new Vector3(cam.orthographicSize * 2 * cam.aspect, transform.localScale.y, 1);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Destroy(col.gameObject);
        }
    }
}
