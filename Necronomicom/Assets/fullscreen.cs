using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullscreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        float y = Camera.main.orthographicSize * 2;
        float x = Camera.main.aspect * y;
       Vector3 camsize = new Vector3(x, y);
        float rendsizey = camsize.y / rend.sprite.bounds.size.y;
        float rendsizex = camsize.x / rend.sprite.bounds.size.x;
        transform.localScale *= rendsizey;
    }
}
