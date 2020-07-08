using UnityEngine;
using System.Collections;

public class Done_RandomRotator : MonoBehaviour 
{
	public float minTumble;
    public float maxTumble;

    void Start ()
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(minTumble, maxTumble);
	}
}