using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private void Awake()
    {
        if(Application.isEditor == false)
            Debug.unityLogger.logEnabled = false;
    }

    public void LoadScene(string sceneName)
    {
        //MyAdsManager.Instance.ShowBanner();
        SceneManager.LoadScene(sceneName);
    }
}
