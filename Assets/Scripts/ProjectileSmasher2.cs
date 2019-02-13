using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSmasher2 : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Destroy(col.gameObject);
        }
    }
}
