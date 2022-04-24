using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastPowerup : MonoBehaviour
{
    public AudioClip collectedClip;
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.FastSpeed(+1);
            Destroy(gameObject);
            
            controller.PlaySound(collectedClip);
        }
    }
}
