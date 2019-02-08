using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public GameObject explosionPrefab;

    public int points;

    EnemyParent enemyParent;

    void Start()
    {
        enemyParent = GetComponentInParent<EnemyParent>();
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
    }

    /// <summary>
    /// Destroys this enemy ship (animation, score update, notify EnemyParent)
    /// </summary>
    public void DestroyShip()
    {
        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        GameManager.Instance.AddScore(points);

        enemyParent.ShipDestroyed();

        Destroy(gameObject);
    }
}
