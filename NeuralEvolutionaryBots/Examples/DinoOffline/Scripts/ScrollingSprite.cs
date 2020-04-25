using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingSprite : MonoBehaviour {

    private Renderer quadRenderer;

    private void Start () {
        quadRenderer = GetComponent<Renderer> ();
    }

    private void Update () {
        var scrollSpeed = DinoManager.singleton.scrollSpeed;
        Vector2 textureOffset = new Vector2 (Time.time * scrollSpeed, 0);
        quadRenderer.material.mainTextureOffset = textureOffset;
    }
}