using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private float yOffset;
    private float zOffset;
    private float loopXPos;

    private void Start()
    {
        yOffset = transform.position.y;
        zOffset = transform.position.z;
    }

    public void SetLoopingPos(float x)
    {
        loopXPos = x;
    }

    public void Scroll(float speed = 0.5f)
    {
        transform.position += new Vector3(Time.deltaTime * speed, 0, 0);

        if (transform.position.x < -loopXPos)
        {
            transform.position = new Vector3(loopXPos, yOffset, zOffset);
        }
        else if (transform.position.x > loopXPos)
        {
            transform.position = new Vector3(-loopXPos, yOffset, zOffset);
        }
    }
}
