using UnityEngine;
using System.Collections;

public class InGameUI : MonoBehaviour {

	private static InGameUI instance;

	private UILabel _instructions;
	private UILabel _score;
	private UILabel _gameOver;
	private UIWidget _leaderboard;
	private UILabel _sweetenerText;


	void Awake()
	{
		instance = this;
	}

	void Start () 
	{
		_score = transform.FindChild("Score").GetComponent<UILabel>();
		_instructions = transform.FindChild("Instructions").GetComponent<UILabel>();
		_gameOver = transform.FindChild("GameOver").GetComponent<UILabel>();
		_leaderboard = transform.FindChild("Leaderboard").GetComponent<UIWidget>();
		_sweetenerText = transform.FindChild("SweetenerText").GetComponent<UILabel>();

		//make the UI look like the game is ready.
		SetGameReadyState();
	}

	public void Init()
	{

	}
	
	public void SetGameInProgressState()
	{
		_score.text = "0";
		_score.gameObject.SetActive(true);
		_instructions.gameObject.SetActive(false);
		_gameOver.gameObject.SetActive(false);
		_leaderboard.gameObject.SetActive(false);
	}

	public void SetGameReadyState()
	{
		_score.gameObject.SetActive(false);
		_instructions.gameObject.SetActive(true);
		_gameOver.gameObject.SetActive(false);
		_leaderboard.gameObject.SetActive(false);
	}

	public void SetGameOverState()
	{
		_score.gameObject.SetActive(false);
		_instructions.gameObject.SetActive(false);
		_gameOver.gameObject.SetActive(true);
		_leaderboard.gameObject.SetActive(false);
	}

	public void SetLeaderboardState()
	{
		_score.gameObject.SetActive(false);
		_instructions.gameObject.SetActive(false);
		_gameOver.gameObject.SetActive(false);
		_leaderboard.gameObject.SetActive(true);
	}

	public void UpdateScore(int score)
	{
		_score.text = score.ToString();
	}

	public void ShowSweetenerText()
	{
		_sweetenerText.gameObject.SetActive(true);

		TweenAlpha tweenAlpha = _sweetenerText.gameObject.GetComponent<TweenAlpha>();
		tweenAlpha.PlayForward();

		TweenScale tweenScale = _sweetenerText.gameObject.GetComponent<TweenScale>();
		tweenScale.PlayForward();
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public static InGameUI Instance {
		get {
			return instance;
		}
	}
}
