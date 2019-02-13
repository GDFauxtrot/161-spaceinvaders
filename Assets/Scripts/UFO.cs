using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public GameObject explosionPrefab;
    public Rigidbody2D rigidbody;
    bool isMovingRight;
    float speed = 1.5f;

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = isMovingRight ? Vector2.right : Vector2.left;
        rigidbody.MovePosition(transform.position + (Vector3)(speed * dir * Time.deltaTime));
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
            Destroy(gameObject);
        }
    }

    public void SetDirection(bool direction)
    {
        isMovingRight = direction;
    }

    public void DestroyShip()
    {
        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        int points;
        int randomPoints = Random.Range(0, 4);
        points = 300 - 50 * randomPoints;
        GameManager.Instance.AddScore(points);

        Destroy(gameObject);
    }
}
