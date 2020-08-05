using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartGame() => SceneManager.LoadScene("Scenes/FieldScene");

    public void LoadProfile() => SceneManager.LoadScene("Scenes/Profile");

    public void LoadStatistic() => SceneManager.LoadScene("Scenes/Statistic");

    public void LoadMm() => SceneManager.LoadScene("Scenes/MainMenu");
}
