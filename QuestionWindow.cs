/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class QuestionWindow : MonoBehaviour {

    private Text questionText;
    private Text SkipQuestion;

    private void Awake() {
        questionText = transform.Find("QuestionText").GetComponent<Text>();
        SkipQuestion = transform.Find("SkipQuestion").GetComponent<Text>();

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void Start() {
        Bird.GetInstance().Question += Bird_Question;
        Hide();
    }

    private void Bird_Question(object sender, System.EventArgs e) {
        questionText.text = Level.GetInstance().GetQuestion();
  
        SkipQuestion.text = "Klik om verder te gaan";

        Show();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

}
