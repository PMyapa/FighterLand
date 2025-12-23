using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager LoadInstance;
    [SerializeField] private GameObject _loadCanvas;

    // Start is called before the first frame update
    void Awake()
    {
        if (LoadInstance == null)
        {
            LoadInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneSync(sceneName));
    }

    IEnumerator LoadSceneSync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        _loadCanvas.SetActive(true);

        while (!operation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        _loadCanvas.SetActive(false);
    }
}
