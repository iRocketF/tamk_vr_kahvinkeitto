using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string difficulty;
    public float goal;

    public Button resetButton;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetDifficulty()
    {

    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
}
