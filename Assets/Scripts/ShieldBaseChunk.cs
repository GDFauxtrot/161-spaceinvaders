using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBaseChunk : MonoBehaviour
{
    static readonly int startHealth = 4;

    int health = startHealth;

    public Animatey animatey;

    void OnCollisionEnter2D(Collision2D col)
    {
        int layer = col.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Projectile"))
        {
            Destroy(col.gameObject);

            if (--health == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                animatey.CycleAnimation();
            }
        }
    }
}
