﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    public BoxCollider2D box;
    public Rigidbody2D rigidbody;
    bool movingRight;
    bool HitWall = false;
    public float speed;

    public GameObject enemyType1, enemyType2, enemyType3;

    GameObject[,] enemies = new GameObject[11, 5];

    Coroutine animCoroutine;

    void Awake()
    {
        PopulateEnemies();
        movingRight = true;

        animCoroutine = StartCoroutine(AnimMovingCoroutine());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;
        if (HitWall)
        {
            dir = dir + Vector2.down*3;
            HitWall = false;
        }
        rigidbody.MovePosition(transform.position + (Vector3)(speed * dir * Time.deltaTime));
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
        HitWall = true;
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
    }
    /// <summary>
    /// Call this function from an Enemy script when callbacks/box needs resizing
    /// </summary>
    public void ShipDestroyed()
    {
        // Calculate new box in case a column has been destroyed
        Vector2 min, max;
        
    }
}
