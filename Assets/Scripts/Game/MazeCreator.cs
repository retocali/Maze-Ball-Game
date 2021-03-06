﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
	Creates the maze in the maze room
 */

//TODO: Make the maze be procedurally generated
public class MazeCreator : MonoBehaviour {

	public GameObject Walls;
	public GameObject Windows;

	// Use this for initialization
	void Start () {
		int[,] verticalMap = new int[,] {
			{1,1,0,1,1,0},
			{0,1,1,0,1,1},
			{1,0,1,1,1,0},
			{0,2,2,2,2,0},
			{0,1,1,1,1,0},
			{0,0,0,1,1,1},
		};

		int[,] horizontalMap = new int[,] {
			{0,0,0,0,0,0},
			{0,0,0,1,0,0},
			{0,0,0,2,0,0},
			{0,1,0,0,0,1},
			{0,0,0,0,0,0},
			{0,1,0,1,0,0},
		};

		float xMax = verticalMap.GetLength(0);
		float yMax = verticalMap.GetLength(1);
		
		for (int x = 0; x < xMax; x++)
		{
			for (int y = 0; y < yMax; y++)
			{
				if (verticalMap[x, y] == 1) {
					var clone = Instantiate(Walls, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
					clone.transform.localPosition = new Vector3(x-xMax/2, 0, y-yMax/2);
				}
				if (verticalMap[x, y] == 2) {
					var clone = Instantiate(Windows, new Vector3(0,0,0), Quaternion.identity, gameObject.transform);
					clone.transform.localPosition = new Vector3(x-xMax/2, 0, y-yMax/2);
				}
				if (horizontalMap[x, y] == 1) {
					var clone = Instantiate(Walls, new Vector3(0,0,0), Quaternion.LookRotation(Vector3.right), gameObject.transform);
					clone.transform.localPosition = new Vector3(x-xMax/2-0.52f, 0, y-yMax/2-0.5f);
				}
				if (horizontalMap[x, y] == 2) {
					var clone = Instantiate(Windows, new Vector3(0,0,0), Quaternion.LookRotation(Vector3.right), gameObject.transform);
					clone.transform.localPosition = new Vector3(x-xMax/2-0.52f, 0, y-yMax/2-0.5f);
				}
				
			}
		}
	}
	
}
