using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string sceneName;

    private void Start()
    {
        button.onClick.AddListener(() => { SceneManager.LoadScene(sceneName);});
    }
}
