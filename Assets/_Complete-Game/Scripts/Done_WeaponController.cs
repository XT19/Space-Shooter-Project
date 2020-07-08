using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Done_WeaponController : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;
	public float minFireRate;
    public float maxFireRate;
    public float delay;

	void Start ()
	{
		InvokeRepeating ("Fire", delay, Random.Range(minFireRate,maxFireRate));
	}

	void Fire ()
	{
		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		GetComponent<AudioSource>().Play();
	}
}
