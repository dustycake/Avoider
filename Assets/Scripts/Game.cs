using UnityEngine;
using System.Collections;
using Core;

public class Game : MonoBehaviour {

	private static float ENEMY_SPAWN_EASY = 1f;
	private static float ENEMY_SPAWN_MEDIUM = 0.85f;
	private static float ENEMY_SPAWN_HARD = 0.7f;
	private static float ENEMY_SPAWN_REALLYHARD = 0.65f;
	private static float ENEMY_SPAWN_INSANE = 0.65f;

	private static Game instance;

	private Player _player;
	private Spawner _spawner;
	private float _laneChangeDuration = 0.35f;
	private string _targetLane = "middle";
	private float _gameTimer = 0.0f;

	private int _score = 0;
	private int _levelScore = 0;
	private Vector3 _playerStartPosition;

	private float _enemySpeed = 0.1f;
	private float _rushCooldownSpeed = 0.005f;

	private string _gameState = "intro";
	private int _gameDifficulty = 1;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		_player = transform.FindChild ("Player").GetComponent<Player>();
		_spawner = transform.FindChild ("Spawner").GetComponent<Spawner>();

		//grab the starting position so we can remember it for when it's time to reset.
		_playerStartPosition = _player.transform.position;

		Reset();
	}

	private void Reset()
	{
		_gameDifficulty = 1;
		_gameState = "intro";
		_gameTimer = 0;
		_player.transform.position = _playerStartPosition;
		_player.gameObject.SetActive(true);
		_targetLane = "middle";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_gameState == "intro")
		{
			//if down, start the game
			if (CheckForInput())
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
			if (CheckForInput())
			{
				_gameState = "leaderboard";
				InGameUI.Instance.SetLeaderboardState();
			}
			return;
		}
		else if (_gameState == "leaderboard")
		{
			//if down, start the game
			if (CheckForInput())
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
			if (CheckForInput())
			{
				ChangeLanes ();
			}
		}
	}

	private void UpdateGame()
	{
		//handle spawning of enemies
		_gameTimer += Time.deltaTime;

		if (_gameTimer >= GetEnemySpawnInterval())
		{
			//catch overflow
			_gameTimer -= GetEnemySpawnInterval();
			
			_spawner.SpawnEnemy();
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
		}
		else if (_targetLane == "right")
		{
			newPosition.x = _spawner.GetSpawnRight().position.x;
		}

		newPosition.y = _player.transform.position.y;

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
		
		//update the score
		IncreaseScore(5);

		//bounce back to the original lane.
		ChangeLanes();

		InGameUI.Instance.UpdateScore(_score);

	}

	public void HandleCoinCollect()
	{
		//update the score
		IncreaseScore(1);
		
		InGameUI.Instance.UpdateScore(_score);

	}

	public void IncreaseScore(int val)
	{
		_score += val;
		_levelScore += val;

		//check for level up
		if (_levelScore >= 100)
		{
			LevelUp();
		}

		RushMeter.Instance.IncreaseRushAmount(val);
	}

	public void LevelUp()
	{
		_levelScore = 0;
		if (_gameDifficulty < 5) 
		{ 
			_gameDifficulty++;
		}

		//change camera color

		InGameUI.Instance.ShowSweetenerText();
	}

	public void SetGameOverState()
	{
		_gameState = "ended";
		InGameUI.Instance.SetGameOverState();
	}

	public float GetEnemySpawnInterval()
	{
		float rVal;
		switch(_gameDifficulty)
		{
		case 1:
			rVal = ENEMY_SPAWN_EASY;
			break;
		case 2:
			rVal = ENEMY_SPAWN_MEDIUM;
			break;
		case 3:
			rVal = ENEMY_SPAWN_HARD;
			break;
		case 4:
			rVal = ENEMY_SPAWN_REALLYHARD;
			break;
		default:
			rVal = ENEMY_SPAWN_INSANE;
			break;
		}
		return rVal;
	}

	public bool CheckForInput()
	{
		if (Input.GetKeyDown ("z") || (Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void BeginRushMode()
	{
		Debug.Log ("Rush mode!");
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

	public int GameDifficulty {
		get {
			return _gameDifficulty;
		}
	}

	public float EnemySpeed {
		get {
			return _enemySpeed;
		}
	}

	public float RushCooldownSpeed {
		get {
			return _rushCooldownSpeed;
		}
	}
}
