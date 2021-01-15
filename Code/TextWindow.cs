using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class TextWindow : MonoBehaviour {

    private Text upperText;
    private Text lowerText;

    /** 
    * Method to construct the textwindow.
    **/
    private void Awake() {
        upperText = transform.Find("UpperText").GetComponent<Text>();
        lowerText = transform.Find("LowerText").GetComponent<Text>();

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    /**
    * Method to hide the textwindow at the start of the game.
    **/
    private void Start() {
        Bird.GetInstance().Answers += Bird_Answers;

        Hide();
    }

    /**
    * Method to show the textwindow with the right text.
    **/
    private void Bird_Answers(object sender, System.EventArgs e) {
        upperText.text = Level.GetInstance().upperAnswer();
  
        lowerText.text = Level.GetInstance().lowerAnswer();

        Show();
    }

    /**
    * Method to hide the textwindow.
    **/
    public void Hide() {
        gameObject.SetActive(false);
    }

    /**
    * Method to show the textwindow.
    **/
    private void Show() {
        gameObject.SetActive(true);
    }

}
