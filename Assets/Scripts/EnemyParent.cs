using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EnemyParent : MonoBehaviour
{
    public BoxCollider2D box;
    public Rigidbody2D rigidbody;
    bool movingRight;
    bool hitWall = false;
    public float speed;
    float shootingTimer, timerBase;
    public GameObject enemyType1, enemyType2, enemyType3, gameManager;
    int min, max, enemiesLeft, level;

    GameObject[,] enemies = new GameObject[11, 5];

    Coroutine animCoroutine;

    public UnityEvent EnemiesAtBottomWall;

    void Awake()
    {
        PopulateEnemies();
        movingRight = true;

        animCoroutine = StartCoroutine(AnimMovingCoroutine());
    }

    // Start is called before the first frame update
    void Start()
    {
        timerBase = 1.0f;
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 dir = movingRight ? Vector2.right : Vector2.left;
        if (hitWall)
        {
            dir = dir + Vector2.down*3;
            hitWall = false;
        }
        rigidbody.MovePosition(transform.position + (Vector3)(speed * dir * Time.deltaTime));
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0)
        {
            ShootProjectile();
            StartTimer();
            speed += 0.05f;
            timerBase -= .005f;
        }
        if (enemiesLeft == 0)
        {
            PopulateEnemies();
            GameManager currManager = gameManager.GetComponent<GameManager>();
            currManager.AddPlayerLife();
        }




    }

    private IEnumerator AnimMovingCoroutine()
    {
        while (true)
        {
            foreach (GameObject go in enemies)
            {
                if (!go)
                    continue;
                Animatey goAnim = go.GetComponent<Animatey>();

                goAnim.CycleAnimation();
            }
            yield return new WaitForSeconds(0.5f / (speed <= 0 ? 0.0000001f : speed));
        }
    }

    public void ChangeDirection()
    {
        movingRight = !movingRight;
        Vector2 dir = Vector2.down;
        hitWall = true;
    }

    public void StartTimer()
    {
        shootingTimer = timerBase + Random.Range(0.0f, 1.0f);
    }

    public void ShootProjectile()
    {
        GameObject bottomEnemy = null;
        int column = Random.Range(min, max + 1);
        for (int y = 0; y< 5; ++y)
        {
            if (enemies[column, y])
                bottomEnemy = enemies[column, y];
        }
        if (bottomEnemy != null)
        {
            Enemy botEnemy = bottomEnemy.GetComponent<Enemy>();
            botEnemy.FireProjectile();
        }


    }
    /// <summary>
    /// Spawns all the enemies under EnemyParent and destroys any previous ones.
    /// </summary>
    public void PopulateEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        level += 1;
        this.transform.position = new Vector3(0, 2 - level * .1f, 0);
        speed = 0.5f;
        timerBase = 1.0f;
        for(int y = 0; y < 5; ++y)
        {
            for (int x = 0; x < 11; ++x)
            {
                GameObject enemy = Instantiate(
                    y == 0 ? enemyType3 : y < 3 ? enemyType2 : enemyType1);

                enemy.transform.SetParent(gameObject.transform);

                enemy.transform.localPosition = new Vector3(x - 5, 2 - y, 0);

                enemies[x, y] = enemy;
            }
        }
        min = 0;
        max = 11;
        enemiesLeft = 55;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BottomWall"))
        {
            EnemiesAtBottomWall.Invoke();
        }
    }

    /// <summary>
    /// Call this function from an Enemy script when callbacks/box needs resizing
    /// </summary>
    public void ShipDestroyed()
    {
        // Calculate new box in case a column has been destroyed
        enemiesLeft--;
        Vector2 min, max;
        
    }
}
