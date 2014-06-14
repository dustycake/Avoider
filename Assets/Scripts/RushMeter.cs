using UnityEngine;
using System.Collections;

public class RushMeter : MonoBehaviour {

	private static RushMeter instance;

	private float _rushAmount = 0;
	private UIProgressBar _meterLeft;
	private UIProgressBar _meterRight;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		_meterLeft = transform.FindChild("MeterLeft").GetComponent<UIProgressBar>();
		_meterLeft.value = 0;
		_meterRight = transform.FindChild("MeterRight").GetComponent<UIProgressBar>();
		_meterRight.value = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_rushAmount > 0 && _rushAmount < 1)
		{
			_rushAmount -= Game.Instance.RushCooldownSpeed;
		}
		if (_rushAmount >= 1)
		{
			_rushAmount = 0;
			Game.Instance.BeginRushMode();
		}

		_meterLeft.value = _rushAmount;
		_meterRight.value = _rushAmount;
	}

	public float RushAmount {
		get {
			return _rushAmount;
		}
	}

	public void IncreaseRushAmount(float val)
	{
		_rushAmount += (val * 0.1f);
	}

	public static RushMeter Instance {
		get {
			return instance;
		}
	}
}
