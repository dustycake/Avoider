using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {


	// Use this for initialization
	void Start () 
	{
		gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	protected virtual void UpdateEnemy () 
	{
		Vector3 newPosition = new Vector3(transform.position.x, transform.position.y - Game.Instance.EnemySpeed);
		transform.position = newPosition;

		if (transform.position.y >= (Screen.height * 0.5f) + 1.0f )
		{
			//destroy
			Destroy (gameObject);

			Debug.Log("Destroyed");
		}
	}
	


}
