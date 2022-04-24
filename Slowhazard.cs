using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowhazard : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.SpeedHazard(+1);
        }
    }
}
