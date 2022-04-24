using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardEnemyController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-2);
        }
    }
    
}