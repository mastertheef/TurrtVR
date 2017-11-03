using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            transform.parent.gameObject.GetComponent<EnemyShip>().TakeDamage(other);
        }
    }
}
