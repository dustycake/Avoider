using UnityEngine;
using System.Collections;
using Core;

public class Game : MonoBehaviour {
	
	private static Game instance;

	private Player _player;
	private Spawner _spawner;
	private float _laneChangeDuration = 0.5f;
	private string _targetLane = "none";
	private float _gameTimer = 0.0f;
	private float _enemySpawnInterval = 2f;
	private int _score = 0;
	private int _levelScore = 0;
	private Vector3 _playerStartPosition;

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

		Reset();
	}

	private void Reset()
	{
		_gameDifficulty = 1;
		_gameState = "intro";
		_gameTimer = 0;
		_player.transform.position = _playerStartPosition;
		_player.gameObject.SetActive(true);
		_targetLane = "none";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_gameState == "intro")
		{
			//if down, start the game
			if (CheckForTap())
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
			if (CheckForTapRelease())
			{
				_gameState = "leaderboard";
				InGameUI.Instance.SetLeaderboardState();
			}
			return;
		}
		else if (_gameState == "leaderboard")
		{
			//if down, start the game
			if (CheckForTap())
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
			HandleIngameInput();
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

	void HandleIngameInput()
	{

		if (Input.GetKeyDown(KeyCode.Z))
		{
			Debug.Log ("Down");
			//the touch has began. Go to the middle lane, but specify which direction youre facing.
			if (_targetLane == "left")
			{
				_targetLane = "middleRight";
			}
			else if (_targetLane == "right")
			{
				_targetLane = "middleLeft";
			}

			ChangeLanes();
		}
		else if (Input.GetKeyUp(KeyCode.Z))
		{
			Debug.Log ("Up");
			if (_targetLane == "middleLeft")
			{
				_targetLane = "left";
			}
			else if (_targetLane == "middleRight")
			{
				_targetLane = "right";
			}

			ChangeLanes();
		}


	}
	
	void ChangeLanes()
	{
		//start the player's particle trail.
		//The tween will turn it off once it's done moving.
		_player.StartParticleSystem();
		
		Vector3 newPosition = new Vector3();
		iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
		if (_targetLane == "left")
		{
			newPosition.x = _spawner.GetSpawnLeft().position.x;
		}
		else if (_targetLane == "right")
		{
			newPosition.x = _spawner.GetSpawnRight().position.x;
		}
		else if (_targetLane == "middleLeft" || _targetLane == "middleRight")
		{
			newPosition.x = _spawner.GetSpawnMiddle().position.x;
			easeType = iTween.EaseType.easeOutExpo;
		}
		else if (_targetLane == "none")
		{
			_targetLane = "right";
			newPosition.x = _spawner.GetSpawnRight().position.x;
		}

		//the y position should stay the same.
		newPosition.y = _playerStartPosition.y;



		//stop all tweens
		iTween.Stop(_player.gameObject);

		Hashtable ht = new Hashtable();
		ht.Add("position", newPosition);
		ht.Add ("time", _laneChangeDuration);
		ht.Add ("easeType", easeType);
		ht.Add ("oncomplete", "LaneChangeComplete");
		//ht.Add ("oncompletetarget", _player.gameObject);
		iTween.MoveTo(_player.gameObject, ht);

	}

	public void LaneChangeComplete()
	{
		//stop the particle system.
		_player.StopParticleSystem();
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

		InGameUI.Instance.ShowSweetenerText();
	}

	public void SetGameOverState()
	{
		_gameState = "ended";
		InGameUI.Instance.SetGameOverState();
	}

	public bool CheckForTap()
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

	public bool CheckForTapRelease()
	{
		if (Input.GetKeyUp ("z") || (Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
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
