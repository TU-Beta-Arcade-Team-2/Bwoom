using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarMask : MaskClass
{
    [SerializeField] private string pathName = "Assets/Sprites/TempSprites/ExampleMask3@4x";

    // Start is called before the first frame update
    void Start()
    {
        GetMaskSprite(pathName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
