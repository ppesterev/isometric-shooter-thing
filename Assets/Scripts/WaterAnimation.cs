using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    MeshRenderer mr = null;
    Vector2 uvOffset = Vector2.zero;
    [SerializeField]
    Vector2 motion = new Vector2(0.1f, 0.02f);
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        uvOffset += motion * Time.deltaTime;
        mr.material.SetTextureOffset("_MainTex", uvOffset);
    }
}
