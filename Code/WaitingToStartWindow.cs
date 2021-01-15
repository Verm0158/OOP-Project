using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingToStartWindow : MonoBehaviour {

    /**
    * Method to start the waitingToStartWindow.
    **/
    private void Start() {
        Bird.GetInstance().OnStartedPlaying += WaitingToStartWindow_OnStartedPlaying;
    }

    /**
    * Method to hide the waitingToStartWindow when the game started playing.
    **/
    private void WaitingToStartWindow_OnStartedPlaying(object sender, System.EventArgs e) {
        Hide();
    }

    /**
    * Method to hide the waitingToStartWindow.
    **/
    private void Hide() {
        gameObject.SetActive(false);
    }

    /**
    * Method to show the waitingToStartWindow.
    **/
    private void Show() {
        gameObject.SetActive(true);
    }

}
