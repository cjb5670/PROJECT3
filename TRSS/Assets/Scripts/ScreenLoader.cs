using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLoader : MonoBehaviour {

    private bool loadScene = false;

    [SerializeField]
    private int scene;
    [SerializeField]
    private Text loadingText;


    // Updates once per frame
    void Update()
    {

        if (loadScene == false)
        {
            loadScene = true;

            loadingText.text = "Generating Roads...";

            StartCoroutine(LoadNewScene());
        }

        if (loadScene == true)
        {

            // Something I found that makes the text flash
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }
    }

    // Loads the scene set in the inspector
    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(2);

        AsyncOperation async = Application.LoadLevelAsync(scene);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
