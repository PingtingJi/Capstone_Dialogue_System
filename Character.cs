using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float speed;
	new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (DialogueManager.isActive == true)
			return;

		float xMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		transform.Translate(new Vector2(xMovement, 0));

		if (Input.GetKeyDown(KeyCode.Space))
		{
			rigidbody.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
		}
	}
}
