using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform Target;

	void Update () {
	    transform.LookAt(Target);
	    transform.RotateAround(Target.position, Vector3.up, -Time.deltaTime * 30);
    }
}
