using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    [SerializeField] private Renderer backgroundRenderer;
    private float yOffset;

    private void OnValidate()
    {
        if (backgroundRenderer == null)
            backgroundRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        yOffset = backgroundRenderer.material.mainTextureOffset.y;
    }

    public void Scroll(float speed = 0.5f)
    {
        backgroundRenderer.material.mainTextureOffset += new Vector2(Time.deltaTime * speed, yOffset);
    }
}
