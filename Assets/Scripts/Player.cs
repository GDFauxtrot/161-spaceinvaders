using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float speed;
    float horizInput;

    bool canFire;
    bool isDead;

    [Header("References")]
    public GameObject projectilePrefab;
    public Rigidbody2D rigidbody;

    void Awake()
    {
        canFire = true;
    }

    void Start()
    {
        GameManager.Instance.player = this;
    }

    void Update()
    {
        #region Movement
        
        horizInput = Input.GetAxisRaw("Horizontal");
        rigidbody.MovePosition(rigidbody.position + Vector2.right * horizInput * speed * Time.deltaTime);

        #endregion

        #region Firing Projectile

        if (Input.GetButtonDown("Fire1") && canFire)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(projectile.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());

            canFire = false;
        }

        #endregion

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        int layer = col.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Projectile"))
        {
            Projectile projectile = col.gameObject.GetComponent<Projectile>();
            if (projectile && projectile.isEnemyProjectile)
            {
                GameManager.Instance.KillPlayer();
            }
        }
    }

    /// <summary>
    /// Turn on shooting for the player if it has been turned off.
    /// </summary>
    public void EnableFire()
    {
        if (!isDead)
        {
            canFire = true;
        }
    }
}
