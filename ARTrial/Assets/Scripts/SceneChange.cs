using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start Game
    public void ButtonClicked()
    {
        SceneManager.LoadScene("AR+GPS");
    }

}
