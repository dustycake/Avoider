using UnityEngine;
using System.Collections;
using Core;

public class Spawner : MonoBehaviour {

	private Transform _spawnLeft;
	private Transform _spawnRight;
	private Transform _spawnMiddle;

	// Use this for initialization
	void Start () 
	{
		ObjectManager.Instance.LoadObject("Prefabs/EnemyLarge");
		ObjectManager.Instance.LoadObject("Prefabs/EnemyLarge2");
		ObjectManager.Instance.LoadObject("Prefabs/EnemyLarge3");
		ObjectManager.Instance.LoadObject("Prefabs/EnemySmall");
		ObjectManager.Instance.LoadObject("Prefabs/EnemyBlock");
		ObjectManager.Instance.LoadObject("Prefabs/EnemyBlockSmall");
		ObjectManager.Instance.LoadObject("Prefabs/EnemyHorizontal");
		ObjectManager.Instance.LoadObject("Prefabs/CoinPatch");

		_spawnLeft = transform.FindChild("SpawnLeft");
		//_spawnLeft.gameObject.SetActive(false);
		_spawnRight = transform.FindChild("SpawnRight");
		//_spawnRight.gameObject.SetActive(false);
		_spawnMiddle = transform.FindChild ("SpawnMiddle");
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void SpawnEnemy()
	{
		switch (Game.Instance.GameDifficulty)
		{
		case 1: SpawnEasy();
			break;
		case 2: SpawnMedium();
			break;
		case 3: SpawnHard();
			break;
		case 4: SpawnReallyHard();
			break;
		default:
			SpawnInsane();
			break;
		}
	}

	private void SpawnEasy()
	{
		//create a random enemy.
		Enemy enemy = GetEnemy(Random.Range (0, 3));
		
		enemy.gameObject.transform.position = GetRandomSpawner().position;
		enemy.gameObject.transform.parent = transform;
	}

	private void SpawnMedium()
	{
		//create a random enemy.
		Enemy enemy = GetEnemy(Random.Range (0, 4));
		
		enemy.gameObject.transform.position = GetRandomSpawner().position;
		enemy.gameObject.transform.parent = transform;
	}

	private void SpawnHard()
	{
		//create a random enemy.
		Enemy enemy = GetEnemy(Random.Range (0, 5));
		
		enemy.gameObject.transform.position = GetRandomSpawner().position;
		enemy.gameObject.transform.parent = transform;
	}

	private void SpawnReallyHard()
	{
		//create a random enemy.
		Enemy enemy = GetEnemy(Random.Range (0, 6));
		
		enemy.gameObject.transform.position = GetRandomSpawner().position;
		enemy.gameObject.transform.parent = transform;
	}

	private void SpawnInsane()
	{
		//create a random enemy.
		Enemy enemy = GetEnemy(Random.Range (0, 7));
		
		enemy.gameObject.transform.position = GetRandomSpawner().position;
		enemy.gameObject.transform.parent = transform;
	}

	public Enemy GetEnemy(int enemyType)
	{
		Enemy enemy;
		switch (enemyType)
		{
		case 0:
			enemy = ObjectManager.Instance.GetLoadedGameObject("Prefabs/CoinPatch").GetComponent<CoinPatch>();
			break;
		case 1:
			enemy = ObjectManager.Instance.GetLoadedGameObject("Prefabs/EnemyBlockSmall").GetComponent<EnemyBlockSmall>();
			break;
		case 2:
			enemy = ObjectManager.Instance.GetLoadedGameObject("Prefabs/EnemySmall").GetComponent<EnemySmall>();
			break;
		case 3:
			enemy = ObjectManager.Instance.GetLoadedGameObject("Prefabs/EnemyBlock").GetComponent<EnemyBlock>();
			break;
		case 4:
			enemy = ObjectManager.Instance.GetLoadedGameObject("Prefabs/EnemyLarge").GetComponent<EnemyLarge>();
			break;
		case 5:
			enemy = ObjectManager.Instance.GetLoadedGameObject("Prefabs/EnemyLarge2").GetComponent<EnemyLarge>();
			break;
		default:
			enemy = ObjectManager.Instance.GetLoadedGameObject("Prefabs/EnemyLarge3").GetComponent<EnemyLarge>();
			break;
		}

		return enemy;
	}

	public Transform GetRandomSpawner()
	{
		//flip left or right or middle side randomly
		int lane = (Random.Range (0, 3));

		//choose a random transform to spawn from.
		Transform flipTrans;
		switch (lane) 
		{
			case 0:
			    flipTrans = _spawnRight;
				break;
			case 1:
				flipTrans = _spawnLeft;
				break;
			default:
				flipTrans = _spawnMiddle;
			    break;
		}

		return flipTrans;
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
