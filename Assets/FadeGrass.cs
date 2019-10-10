using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeGrass : MonoBehaviour
{

    List<Material> mats = new List<Material>();
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mr in mrs)
        {
            foreach(Material mat in mr.materials)
            {
                mats.Add(mat);
            }
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    // TODO actually fade over time
    public void Fade()
    {
        foreach(Material mat in mats)
        {
            Color c = mat.color;
            c.a = 0.2f;
            mat.color = c;
        }
    }
}
