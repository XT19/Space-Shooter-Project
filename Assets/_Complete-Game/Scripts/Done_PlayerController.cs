using UnityEngine;
using System.Collections;

//System serializable pour que unity prenne en compte la classe comme classe interne
[System.Serializable]
//Classe possedantt le groupe de variable pour limiter les mouvements du joueur
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
    //Ces variable seront visible dans la composante du script de unity
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

    //ces variables servent a acceuilir l'endroit ou va apparaitre le tire et l'objet du tire en soit
	public GameObject shot;
	public Transform shotSpawn;
    //cadance de tire
	public float fireRate;
	 
	private float nextFire;
	
	void Update ()//s'execute a chaque frame aka tout le temps
	{
        //Boutton Fire1 est predifinit c'est le boutton Ctrl
        //ne tire que quand le temps actuelle depasse nexFire
        if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
            //nextFire est egale au temps actuelle + la cadence de tire
			nextFire = Time.time + fireRate;
            //Fait apparaitre Shot dans la postion du shotSpawn avec sa rotation ses deux objets etant les var public en haut
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //Recupère la son contenu dans la composante AudioSource de l'objet actuelle et le joue
			GetComponent<AudioSource>().Play ();
		}
	}
    
	void FixedUpdate ()//S'execure soit une fois, soit plusieurs fois soit pas du tout chaque frame
	{
        //GetAxis Horizontal est predefinit et veut dire les fleches directionelles
        float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

        //Declaration d'un nouveau vecteur carthesin de mouvement sur l'axe X et Z seuelement
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        //Modification de la velocité de l'objet selon le vecteur de mouvement multiplié par une vitesse
        //( car le get axis genère seulement une valeur float entre 0 et 1 et faut l'emplifier
		GetComponent<Rigidbody>().velocity = movement * speed;
		
        //Recupère la position actuelle de l'objet et la compare par rapport a deux valeurs
		GetComponent<Rigidbody>().position = new Vector3
		(
            //Mathf.calmp force la première valeur a rester dans le champ des deux autres
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, //le Y importe pas
            //meme chose pour le Z on donne une frontière pour le limiter
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		//Finalement pour plus d'esthetique on modifie la rotation de l'objet actuelle selon son mouvement
        //le Quaternion.Euler modifie l'inclinaison selon l'axe Z par rapport a la velocité de l'objet
        //et ce en l'amplifiant fois un nombre negatif afin qu'il tourne dans le sens inverse et qu'il donne
        //l'impression de se pencher dans le sens de la direction, le tilt definit de combiens de degrèe ce dernier
        //va se pencher
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}
}
