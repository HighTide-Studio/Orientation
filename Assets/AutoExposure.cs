using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AutoExposiure : MonoBehaviour
{
    public Volume NrmVolume, ExposedVolume;
    public Light sun;
    public GameObject SightEnd;

    public LayerMask OpaqueLayers;

    public float SightDist = 5;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(SightEnd.transform.position, -sun.transform.forward, out hit, SightDist, OpaqueLayers))
        {
            NrmVolume.weight -= .05f * Time.deltaTime;
            if (NrmVolume.weight < 0f)
            {
                NrmVolume.weight = 0f; 
            }
            ExposedVolume.weight += .05f * Time.deltaTime;
            if (ExposedVolume.weight > 1f)
            {
                ExposedVolume.weight = 1f;
            }
        }
        else
        {
            NrmVolume.weight += .05f * Time.deltaTime;
            if (NrmVolume.weight > 1f)
            {
                NrmVolume.weight = 1f;
            }
            ExposedVolume.weight -= .05f * Time.deltaTime;
            if (ExposedVolume.weight < 0f)
            {
                ExposedVolume.weight = 0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, SightEnd.transform.position);
        Gizmos.DrawLine(SightEnd.transform.position, -sun.transform.forward);
    }
}