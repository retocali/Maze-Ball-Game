﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Player movement constants
	public float speed;
	public float jumpHeight;
	public float minHeight = -50f;
	
	public float scaleFactor = 0.3f;
	public int boxLayer = 1 << 9;
	
	public GameObject bubblePrefab;

	public Camera mainCamera;
	
	public AudioClip bubble;
	public AudioClip jump;
	
	public AudioSource audioBubble;
	public AudioSource audioJump;

	
	// Internal variables
	public float pickUpDistance = 2;
	private Rigidbody rb;
	
	private bool onGround;
	private bool holding;
	
	private GameObject item;
	private GameObject currentBubble;

	private Vector3 initialPosition;
	

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		onGround = false;
		holding = false;
		initialPosition = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		// Moving
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			moveRight(Time.deltaTime);
		} 
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			moveLeft(Time.deltaTime);
		} 
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			moveUp(Time.deltaTime);
		} 
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			moveDown(Time.deltaTime);
		} 

		// Jumping
		if (Input.GetKey(KeyCode.Space) && onGround) {
			audioJump.clip = jump;
			audioJump.Play();
			rb.velocity	 = jumpHeight * - Physics.gravity.normalized;

		}

		// Item holding
		if (Input.GetKeyDown(KeyCode.E)) {
			if (!holding) {
				pickUp();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Q)) {
			if (holding) {
				drop();
			}
		}
		moveItem();

		// Checking for Jumps
		if (Physics.Raycast(transform.position, Physics.gravity.normalized, transform.lossyScale.y)) {
			onGround = true;
		} else {
			onGround = false;
		}

		// Respawn if the player falls out of bounds
		if (transform.position.y <= minHeight) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			transform.position = initialPosition;
		}
			
	}

		// Extra update to solve jitteriness
	void LateUpdate()
	{
		mainCamera.transform.position = transform.position - Physics.gravity.normalized/2;	
		if (holding) {
			currentBubble.transform.position = item.GetComponent<Rigidbody>().transform.position;
		}
	}

	RaycastHit hit;

	void pickUp() {
		
		if (Physics.BoxCast(transform.position, 2*(Vector3.one-Vector3.forward), 
				pickUpDistance * mainCamera.transform.TransformVector(Vector3.forward),
				out hit, mainCamera.transform.rotation, 1, boxLayer)) {
				
				audioBubble.PlayOneShot(bubble, 0.9F);
			
			if (!(hit.collider.gameObject.tag == "Box")) {
				return;	
			}

			holding = true;
			item = hit.collider.gameObject;

			item.GetComponent<Rigidbody>().useGravity = false;
			item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			
			item.transform.localScale *= scaleFactor;
			makeBubble();
			Physics.IgnoreCollision(item.GetComponent<Collider>(), GetComponent<Collider>(), true);
		}
	
	}
	
	void drop() {
		if (holding && item.GetComponent<BoxController>().isDroppable()) {
			item.GetComponent<Collider>().isTrigger = false;
			item.GetComponent<Rigidbody>().useGravity = true;
			
			item.transform.localScale /= scaleFactor;
			
			Destroy(currentBubble);
			
			audioBubble.PlayOneShot(bubble, 0.9F);

			Physics.IgnoreCollision(item.GetComponent<Collider>(), GetComponent<Collider>(), false);
			
			item = null;
			holding = false;
			
		}
		
	}

	void moveItem() {
		if (holding) {
			Vector3 heldPosition = transform.position + mainCamera.transform.TransformVector(transform.forward*1.5f);
			Vector3 distance = heldPosition - item.GetComponent<Rigidbody>().position;

			float minimum_distance = 0.1f;
			float maximum_distance = 5f;
			
			if (distance.magnitude > maximum_distance) 
			{
				drop();
			} 
			else if (distance.magnitude > minimum_distance) 
			{
				item.GetComponent<Rigidbody>().velocity = (speed*speed/2 * distance);
			} else 
			{
				item.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}
	}
	public void teleportItem(Vector3 position) {
		if (holding) {
			item.GetComponent<Collider>().isTrigger = true;
			item.GetComponent<Rigidbody>().position = position + mainCamera.transform.TransformVector(transform.forward*1.5f);
			item.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	void moveRight(float time) {
		Vector3 movement = mainCamera.transform.TransformVector(speed * transform.right * time);
		rb.position += new Vector3(movement.x, 0, movement.z);
	}
	void moveLeft(float time) {
		Vector3 movement = mainCamera.transform.TransformVector(speed * transform.right * time);
		rb.position -= new Vector3(movement.x, 0, movement.z);
	}
	void moveUp(float time) {
		Vector3 movement = mainCamera.transform.TransformVector(speed * transform.forward * time);
		rb.position += new Vector3(movement.x, 0, movement.z);
	}
	void moveDown(float time) {
		Vector3 movement = mainCamera.transform.TransformVector(speed * transform.forward * time);
		rb.position -= new Vector3(movement.x, 0, movement.z);
	}
		
	void makeBubble(){
		currentBubble = Instantiate(bubblePrefab);
	}
	
	public bool isHolding() {
		return holding;
	}
}

	