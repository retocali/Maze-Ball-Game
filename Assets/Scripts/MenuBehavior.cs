﻿using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InterSceneData;

public class MenuBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public Text theText;

	void Start(){
		if (transform.name == "Start") {
			if (Data.gameLoaded) {
				GetComponentInChildren<Text>().text = "Resume";
			}
		}
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		theText.color = Color.cyan; 
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		theText.color = Color.black; 
	}

	public void OnPointerDown(PointerEventData eventData) {
		if (SceneManager.GetActiveScene().name != "Menu") {
			// Buttons should do nothing if the menu is not the active scene
			return;
		}
		
		if (transform.name == "Start") {
			if (SceneManager.sceneCount == 1) {
				// Load the game for the first time
				Data.gameLoaded = true;
				SceneManager.LoadScene("Main Game");

			} else {
				// Go back to the previous game
				SceneManager.UnloadSceneAsync("Menu");
				SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main Game"));
			}
			
		}

		if (transform.name == "How To Play" ) {
			StartCoroutine(setScene("Instructions"));
		}

		if (transform.name == "Setting" ) {
			StartCoroutine(setScene("Setting"));
		}
	}

	IEnumerator setScene(string scene) {
		AsyncOperation async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		while (!async.isDone) {
			yield return new WaitForEndOfFrame();
		}	
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
	}

}