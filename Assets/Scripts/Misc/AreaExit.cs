using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string targetEntrance;

    private float waitToLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerController>())
        {
            SceneManagement.Instance.SetEntranceName(targetEntrance);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
    }
    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
