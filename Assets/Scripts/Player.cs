using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Player : MonoBehaviour
{
    public static Player Instance;

    public float speed;
    float horizInput;

    bool canFire;
    bool isDead;

    [Header("References")]
    public GameObject projectilePrefab;
    public Rigidbody2D rigidbody;

    public UnityEvent PlayerDeath;

    void Awake()
    {
        canFire = true;
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.player = this;
    }

    void Update()
    {
        #region Pause

        if (Input.GetButtonDown("Cancel") && !isDead)
        {
            GameManager.Instance.ToggleGamePause();
        }

        #endregion

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
                PlayerDeath.Invoke();
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
