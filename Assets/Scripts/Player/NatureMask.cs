using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureMask : MaskClass
{
    [SerializeField] private string pathName = "Sprites/TempSprites/ExampleMask2@4x";

    // Start is called before the first frame update
    void Start()
    {
        maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        GetMaskSprite(pathName);
    }
}
