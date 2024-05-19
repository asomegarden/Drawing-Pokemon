using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Vector3 destination;
    public GameObject target;

    public void TriggerWarp()
    {
        target.transform.position = destination;
    }
}