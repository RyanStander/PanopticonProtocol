using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private Renderer backgroundRenderer;

    private void OnValidate()
    {
        if (backgroundRenderer == null)
            backgroundRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        backgroundRenderer.material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }
}
