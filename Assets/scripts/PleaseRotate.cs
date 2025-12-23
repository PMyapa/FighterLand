using UnityEngine;

public class PleaseRotate : MonoBehaviour
{
    [SerializeField] private GameObject CanvasObject;

    private static PleaseRotate instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (CanvasObject != null)
        {
            CanvasObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.height > Screen.width)
        {
            if (CanvasObject != null && !CanvasObject.activeSelf)
            {
                CanvasObject.SetActive(true);
            }
        }
        else
        {
            if (CanvasObject != null && CanvasObject.activeSelf)
            {
                CanvasObject.SetActive(false);
            }
        }
    }
}
