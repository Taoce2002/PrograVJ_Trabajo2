using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    [SerializeField]
    private float damageMelee = 50f;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            other.GetComponent<EnemyController>().ReceiveDamage(damageMelee);
        }
    }
}
