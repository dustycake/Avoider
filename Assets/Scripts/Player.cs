using UnityEngine;
using System.Collections;
using Core;

public class Player : MonoBehaviour {

	ParticleSystem _particleSystem;

	// Use this for initialization
	void Start () 
	{
		ObjectManager.Instance.LoadObject("Particles/FX_EnemyDestroy");

		//grab the particle trail and stop it.
		_particleSystem = transform.FindChild("FX_PlayerTrail").GetComponent<ParticleSystem>();
		StopParticleSystem();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartParticleSystem()
	{
		_particleSystem.Play ();
	}

	public void StopParticleSystem()
	{
		_particleSystem.Stop();
	}

	private void OnTriggerEnter (Collider col) 
	{
		if (col.gameObject.tag == "Enemy")
		{
			//find out what it collided with. If game over, then game over.
			if (col.gameObject.name == "EnemySphere")
			{
				//show particle
				GameObject particle = ObjectManager.Instance.GetLoadedGameObject("Particles/FX_EnemyDestroy");
				particle.transform.parent = transform.parent;
				particle.transform.position = col.transform.position;

				//you can destroy the enemy.
				Destroy(col.transform.parent.gameObject);

				Game.Instance.HandleEnemyDestroy();
			}
			else if (col.gameObject.name == "EnemyBox")
			{
				//destroy the player.
				gameObject.SetActive(false);

				//the game is over.
				Game.Instance.SetGameOverState();
			}
		}

	}
}
