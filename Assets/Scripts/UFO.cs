using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UFO : MonoBehaviour
{
    public GameObject explosionPrefab;
    public Rigidbody2D rigidbody;
    public float speed;

    public float spawnXPos, spawnYPos;

    int movingDir;

    public float spawnUFOTimer;

    void Start()
    {
        GameManager.Instance.player.playerDeathEvent.AddListener(OnPlayerDeath);
        StartTimer();
    }

    void Update()
    {
        spawnUFOTimer -= Time.deltaTime;
        if (spawnUFOTimer <= 0)
        {
            int rand = Random.Range(0, 2);
            StartFlying(rand == 0 ? -1 : 1);

            StartTimer();
        }

        movingDir = Mathf.Clamp(movingDir, -1, 1);
        Vector2 dir = Vector2.right * movingDir;
        rigidbody.MovePosition(transform.position + (Vector3)(speed * dir * Time.deltaTime));
    }

    public void StartTimer()
    {
        spawnUFOTimer = 15 + Random.Range(0f, 15f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        int layer = col.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Projectile"))
        {
            Projectile projectile = col.gameObject.GetComponent<Projectile>();
            if (projectile && !projectile.isEnemyProjectile)
            {
                DestroyShip();
                Destroy(col.gameObject);
            }
        }
        if (layer == LayerMask.NameToLayer("Walls"))
        {
            StartFlying(0);
        }
    }

    /// <summary>
    /// Tells the UFO which way to fly. 1 for right, -1 for left, anything else for no moving
    /// </summary>
    /// <param name="dir"></param>
    public void StartFlying(int dir)
    {
        movingDir = dir;

        if (movingDir == -1)
        {
            // Moving left -- start right
            transform.position = new Vector3(spawnXPos, spawnYPos, 0);
        }
        else if (movingDir == 1)
        {
            // Moving right -- start left
            transform.position = new Vector3(-spawnXPos, spawnYPos, 0);
        }
        else
        {
            // Not moving (invalid dir? was made 0 on purpose?)
            movingDir = 0;
            transform.position = new Vector3(0, 10, 0);
        }
    }

    async void OnPlayerDeath()
    {
        spawnUFOTimer = 999;

        StartFlying(0);

        await Task.Delay(GameManager.Instance.player.deathPauseTimeMs);

        StartTimer();
    }

    public void DestroyShip()
    {
        Debug.Log("Destroyed!");

        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Adds score in the range of 100 to 300 in increments of 50
        GameManager.Instance.AddScore(300 - 50 * Random.Range(0, 5)); // not 4, the max is exclusive

        StartFlying(0);
    }
}
