using UnityEngine;
using System.Collections;
using Core;

public class Player : MonoBehaviour {

	private ParticleSystem _particleSystem;

	// Use this for initialization
	void Start () 
	{

		//load particle systems.
		ObjectManager.Instance.LoadObject("Particles/FX_EnemyDestroy");
		ObjectManager.Instance.LoadObject("Particles/FX_CoinCollect");
		ObjectManager.Instance.LoadObject("Particles/FX_PlayerDestroy");

		//grab the particle trail and stop it.
		_particleSystem = transform.FindChild("FX_PlayerTrail").GetComponent<ParticleSystem>();
		StopParticleSystem();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void StartParticleSystem()
	{
		_particleSystem.Play ();
	}

	public void StopParticleSystem()
	{
		_particleSystem.Stop();
	}

	//handle collision of objects.
	private void OnTriggerEnter (Collider col) 
	{
		return;
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

				//pass in the name to make sure it's not a horizontal one
				Game.Instance.HandleEnemyDestroy();
			}
			else if (col.gameObject.name == "Coin")
			{
				Debug.Log ("Coin Collected");

				//show particle
				GameObject particle = ObjectManager.Instance.GetLoadedGameObject("Particles/FX_CoinCollect");
				particle.transform.parent = transform.parent;
				particle.transform.position = col.transform.position;

				//you can destroy the enemy.
				Destroy(col.transform.gameObject);

				//collect the coin
				Game.Instance.HandleCoinCollect();
			}
			else if (col.gameObject.name == "EnemyBox")
			{
				//destroy the player.
				gameObject.SetActive(false);

				//show particle
				GameObject particle = ObjectManager.Instance.GetLoadedGameObject("Particles/FX_PlayerDestroy");
				particle.transform.parent = transform.parent;
				particle.transform.position = col.transform.position;

				//the game is over.
				Game.Instance.SetGameOverState();
			}

		}

	}
}
