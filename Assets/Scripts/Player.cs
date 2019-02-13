using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

public class Player : MonoBehaviour
{
    public int deathPauseTimeMs;
    public int deathPauseInvisibleMs;

    public float speed;
    float horizInput;

    bool canFire;
    bool isDead;

    [Header("References")]
    public GameObject projectilePrefab;
    public Rigidbody2D rigidbody;

    public UnityEvent playerDeathEvent;

    void Awake()
    {
        canFire = true;

        GameManager.Instance.player = this;
    }

    void Start()
    {
        playerDeathEvent.AddListener(OnPlayerDeath);
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

        horizInput = isDead ? 0 : Input.GetAxisRaw("Horizontal");
        rigidbody.MovePosition(rigidbody.position + Vector2.right * horizInput * speed * Time.deltaTime);

        #endregion

        #region Firing Projectile

        if (Input.GetButtonDown("Fire1") && !isDead && canFire)
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
                // GameManager.Instance.KillPlayer();
                playerDeathEvent.Invoke();
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

    async void OnPlayerDeath()
    {
        SpriteRenderer sprRender = GetComponent<SpriteRenderer>();
        Sprite shipSpr = sprRender.sprite;

        // Turn on death anim for player, let it play for a second
        isDead = true;
        Animatey anim = GetComponent<Animatey>();

        anim.useCoroutine = true;
        anim.ResetCoroutine();

        await Task.Delay(deathPauseTimeMs - deathPauseInvisibleMs);

        anim.useCoroutine = false;
        anim.ResetCoroutine();

        if (GameManager.Instance.GetLives() > 0)
        {
            // Disappear for a half second then respawn, re-enabling player
            sprRender.sprite = null;

            await Task.Delay(deathPauseInvisibleMs);

            sprRender.sprite = shipSpr;
            isDead = false;
            canFire = true;
        }

    }
}
