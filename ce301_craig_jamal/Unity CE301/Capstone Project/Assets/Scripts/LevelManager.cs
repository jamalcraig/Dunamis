using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public Node[,] racerGrid;
    public Node[,] playerGrid;
    public Node[,] cloneGrid;

    public void RestartScene() {
        Scene s = SceneManager.GetActiveScene();
        SceneManager.LoadScene(s.buildIndex);
    }

    
}
