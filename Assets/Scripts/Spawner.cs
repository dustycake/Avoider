using UnityEngine;
using System.Collections;
using Core;

public class Spawner : MonoBehaviour {

	private Transform _spawnLeft;
	private Transform _spawnRight;

	// Use this for initialization
	void Start () 
	{
		ObjectManager.Instance.LoadObject("Prefabs/Enemy");

		_spawnLeft = transform.FindChild("SpawnLeft");
		//_spawnLeft.gameObject.SetActive(false);
		_spawnRight = transform.FindChild("SpawnRight");
		//_spawnRight.gameObject.SetActive(false);


	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void SpawnEnemy(int gameDifficulty)
	{
		//create an enemy, the type of which depends upon the difficulty
		Enemy enemy = ObjectManager.Instance.GetLoadedGameObject("Prefabs/Enemy").GetComponent<Enemy>();

		//flip left or right side randomly
		bool flip = (Random.value <= 0.5f);
		Transform flipTrans;
		if (flip) 
		{
			flipTrans = _spawnLeft;
		} 
		else 
		{
			flipTrans = _spawnRight;
		}

		enemy.gameObject.transform.position = flipTrans.transform.position;
		enemy.gameObject.transform.parent = transform;

	}
	
	public Transform GetSpawnLeft ()
	{
		return _spawnLeft;
	}

	public Transform GetSpawnRight()
	{
		return _spawnRight;
	}
}
