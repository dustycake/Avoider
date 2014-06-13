using UnityEngine;
using System.Collections;
using Core;

public class Game : MonoBehaviour {

	private static float MOVE_FORWARD_AMOUNT = 0.2f;

	private static Game instance;

	private Player _player;
	private Spawner _spawner;
	private float _laneChangeDuration = 0.35f;
	private string _targetLane = "middle";
	private float _gameTimer = 0.0f;
	private float _enemySpawnInterval = 0.75f;
	private int _score = 0;
	private int _levelScore = 0;
	private Vector3 _playerStartPosition;
	private float _movedForwardPosition = 0.0f;

	//states
	private string _gameState = "intro";
	private int _gameDifficulty = 1;

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

		//grab just the forwards position.
		_movedForwardPosition = _player.transform.position.y;

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
		_movedForwardPosition = _playerStartPosition.y;
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

		if (_gameTimer >= _enemySpawnInterval)
		{
			//catch overflow
			_gameTimer -= _enemySpawnInterval;
			
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

		//set the y position
		newPosition.y = _movedForwardPosition;
		
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
		_score += 5;
		_levelScore += 5;

		//see if it's time to level up
		//CheckForLevelUp();

		if (_levelScore >= 100)
		{
			LevelUp();
		}
		else
		{
			//make it so that the player will move forwards a bit every time you score.
			_movedForwardPosition += MOVE_FORWARD_AMOUNT;
		}

		//go back to the original lane.
		ChangeLanes();


		InGameUI.Instance.UpdateScore(_score);

	}

	public void HandleCoinCollect()
	{
		//update the score
		_score += 1;
		_levelScore += 1;

		//see if it's time to level up;
		//CheckForLevelUp();

		if (_levelScore >= 100)
		{
			LevelUp();
		}
		
		InGameUI.Instance.UpdateScore(_score);

	}

	public void LevelUp()
	{
		_levelScore = 0;
		_movedForwardPosition = _playerStartPosition.y;

		InGameUI.Instance.ShowSweetenerText();
	}

	public void SetGameOverState()
	{
		_gameState = "ended";
		InGameUI.Instance.SetGameOverState();
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
}
