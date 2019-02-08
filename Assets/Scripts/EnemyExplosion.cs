using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public Animatey animatey;
    public float timeToDestroy;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
