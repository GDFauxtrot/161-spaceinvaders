using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public bool isEnemyProjectile;
    public float speed;

    void Update()
    {
        int direction = isEnemyProjectile ? -1 : 1;
        rigidbody.MovePosition(rigidbody.position + Vector2.up * speed * direction * Time.deltaTime);
    }

    void OnDestroy()
    {
        if (!isEnemyProjectile)
        {
            Player player = GameManager.Instance.player;
            player.EnableFire();
        }
    }
}
