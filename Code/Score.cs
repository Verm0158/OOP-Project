using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Represents the score.
**/
public static class Score {

    /**
    * Method to start the score.
    **/
    public static void Start() {
        //ResetHighscore();
        Bird.GetInstance().OnDied += Bird_OnDied;
    }

    /**
    * Method to set the new highscore if the bird died.
    **/
    private static void Bird_OnDied(object sender, System.EventArgs e) {
        TrySetNewHighscore(Level.GetInstance().GetPipesPassedCount());
    }

    /**
    * Getter for the highscore.
    **/
    public static int GetHighscore() {
        return PlayerPrefs.GetInt("highscore");
    }

    /**
    * Method to set the new highscore.
    **/
    public static bool TrySetNewHighscore(int score) {
        int currentHighscore = GetHighscore();
        if (score > currentHighscore) {
            // New Highscore
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        } else {
            return false;
        }
    }

    /**
    * Method to reset the highscore.
    **/
    public static void ResetHighscore() {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
    }
}
