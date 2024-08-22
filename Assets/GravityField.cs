using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    //Determines if gravity vector is calculated by distance to center of object
    public bool isDynamic = false;

    //Determines if gravity direction is flipped for inverted shapes
    public bool isDirectionFlipped = false;

    //The lower, the higher the priority of the gravity field
    [SerializeField] private int priority = 0;

    //Strength in Gravity
    public float gravityStrength = 9.81f;

    //Direction of Gravity, is world down by default
    public Vector3 gravityDir = Vector3.down;

    //Overall Gravity Vector
    public Vector3 gravityVector;

    void Start()
    {
        gravityVector = gravityDir * gravityStrength;

        if (!isDynamic)
        {
            gravityDir = -this.gameObject.transform.parent.GetChild(0).up;
            gravityVector = gravityDir * gravityStrength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetGravityVector(Transform otherObj)
    {
        if (isDynamic)
        {
            Vector3 gravityPosition = this.gameObject.transform.parent.gameObject.transform.position;
            Vector3 objectPosition = otherObj.position;

            //Computes Direction Vector, normalized
            gravityDir = (new Vector3(gravityPosition.x - objectPosition.x, gravityPosition.y - objectPosition.y, gravityPosition.z - objectPosition.z).normalized);
            gravityVector = gravityDir * gravityStrength;
        }
        if (!isDirectionFlipped)
        {
            return gravityVector;
        }
        else
        {
            return -gravityVector;
        }
    }
}
