using UnityEngine;
using System.Collections;
using Core;

public class Game : MonoBehaviour {

	private static Game instance;

	private Player _player;
	private Spawner _spawner;
	private float _laneChangeDuration = 0.35f;
	private string _targetLane = "middle";
	private float _gameTimer = 0.0f;
	private float _enemySpawnInterval = 1.0f;
	private int _score = 0;
	private Vector3 _playerStartPosition;

	//states
	private string _gameState = "intro";
	private int _gameDifficulty = 0;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {

		_player = transform.FindChild ("Player").GetComponent<Player>();
		_spawner = transform.FindChild ("Spawner").GetComponent<Spawner>();

		//grab the starting position so we can remember it for when it's time to reset.
		_playerStartPosition = _player.transform.position;

		Reset();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_gameState == "intro")
		{
			//if down, start the game
			if (Input.GetKeyDown ("z"))
			{
				//we are now playing!
				_gameState = "playing";
				InGameUI.Instance.SetGameInProgressState();
				ChangeLanes ();
			}
			return;
		}
		else if (_gameState == "ended")
		{
			//if down, show the leaderboard
			if (Input.GetKeyDown ("z"))
			{
				_gameState = "leaderboard";
				InGameUI.Instance.SetLeaderboardState();
			}
			return;
		}
		else if (_gameState == "leaderboard")
		{
			//if down, start the game
			if (Input.GetKeyDown ("z"))
			{
				//we are now playing!
				_gameState = "intro";
				Reset();
				InGameUI.Instance.SetGameReadyState();
			}
			return;
		}
		else if (_gameState == "playing")
		{

			UpdateGame();

			//if down, start the game
			if (Input.GetKeyDown ("z"))
			{
				ChangeLanes ();
			}
		}
	}

	private void Reset()
	{
		_gameDifficulty = 0;
		_gameState = "intro";
		_gameTimer = 0;
		_player.transform.position = _playerStartPosition;
		_player.gameObject.SetActive(true);
		_targetLane = "middle";
	}

	private void UpdateGame()
	{
		//handle spawning of enemies
		_gameTimer += Time.deltaTime;

		if (_gameTimer >= _enemySpawnInterval)
		{
			//catch overflow
			_gameTimer -= _enemySpawnInterval;
			
			_spawner.SpawnEnemy(_gameDifficulty);
		}
	}
	
	void ChangeLanes()
	{
		//start the player's particle trail.
		//The tween will turn it off once it's done moving.
		_player.StartParticleSystem();
		
		switch (_targetLane)
		{
		case "middle":
		case "left":
			_targetLane = "right";
			break;
		case "right":
			_targetLane = "left";
			break;
		}
		
		Vector3 newPosition = new Vector3();
		if (_targetLane == "left")
		{
			newPosition.x = _spawner.GetSpawnLeft().position.x;
			newPosition.y = _player.transform.position.y;
		}
		else if (_targetLane == "right")
		{
			newPosition.x = _spawner.GetSpawnRight().position.x;
			newPosition.y = _player.transform.position.y;
		}

		Hashtable ht = new Hashtable();
		ht.Add("position", newPosition);
		ht.Add ("time", _laneChangeDuration);
		ht.Add ("easeType", iTween.EaseType.easeOutQuart);
		ht.Add ("oncomplete", "StopParticleSystem");
		ht.Add ("oncompletetarget", _player.gameObject);
		iTween.MoveTo(_player.gameObject, ht);

	}

	public void HandleEnemyDestroy()
	{
		//bounce the player back.
		iTween.Stop(_player.gameObject);

		//go back to the original lane.
		ChangeLanes();

		//update the score
		_score += 5;
		InGameUI.Instance.UpdateScore(_score);
	}

	public void SetGameOverState()
	{
		_gameState = "ended";
		InGameUI.Instance.SetGameOverState();
	}
	
	public static Game Instance {
		get 
		{
			return instance;
		}
	}

	public float GameTimer {
		get 
		{
			return _gameTimer;
		}
	}
}
