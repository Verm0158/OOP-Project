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

public class TextWindow : MonoBehaviour {

    private Text upperText;
    private Text lowerText;

    private void Awake() {
        upperText = transform.Find("UpperText").GetComponent<Text>();
        lowerText = transform.Find("LowerText").GetComponent<Text>();

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void Start() {
        Bird.GetInstance().Answers += Bird_Answers;

        Hide();
    }

    private void Bird_Answers(object sender, System.EventArgs e) {
        upperText.text = Level.GetInstance().upperAnswer();
  
        lowerText.text = Level.GetInstance().lowerAnswer();

        Show();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

}
