using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeRotateGear : MonoBehaviour
{
   private void Update()
    {
        transform.Rotate(0, 0, -3f * Time.deltaTime);
    }
}
