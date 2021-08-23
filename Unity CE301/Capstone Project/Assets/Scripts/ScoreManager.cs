using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI dunamisAmmoText;
    public TextMeshProUGUI healthText;
    public int dunamisAmmo;
    public static int score;
    public static int killedRacerScore = 100;
    public int dunamisScore = 10;
    public int pipeDunamisScore = 50;
    public int health;

    private void Start() {
        score = 0;
        UpdateText();
    }

    void UpdateText() {
        scoreText.SetText("Score: " + score);
        dunamisAmmoText.SetText("x" + dunamisAmmo);
        healthText.SetText("Health: " + health);
    }

    public void AddScore(int s) {
        score += s;
        UpdateText();
    }

    public void KilledRacer() {
        AddScore(killedRacerScore);
    }

    public void PickedUpDunamis(int i) {
        AddScore(dunamisScore * i);
    }

    public void PickedUpPipeDunamis(int i) {
        AddScore(pipeDunamisScore * i);
    }

    public void UpdateDunamisAmmoCount(int d) {
        dunamisAmmo = d;
        UpdateText();
    }

    public void UpdateHealth(int h) {
        health = h;
        UpdateText();
    }

}
