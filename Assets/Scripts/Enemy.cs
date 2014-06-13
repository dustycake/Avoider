using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float _speed = 0.1f;

	// Use this for initialization
	void Start () 
	{
		gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	protected virtual void UpdateEnemy () 
	{
		Vector3 newPosition = new Vector3(transform.position.x, transform.position.y - _speed);
		transform.position = newPosition;

		if (transform.position.y >= (Screen.height * 0.5f) + 1.0f )
		{
			//destroy
			Destroy (gameObject);

			Debug.Log("Destroyed");
		}
	}

	public void SetSpeed()
	{
		_speed = 0.1f;
	}


}
