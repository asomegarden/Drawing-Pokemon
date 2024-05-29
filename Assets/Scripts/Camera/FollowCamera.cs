using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        transform.position = target.position + Vector3.back * 10;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);

    private void OnPreRender()
    {
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Clear(true, true, Color.black);
        GL.PopMatrix();
    }
}
