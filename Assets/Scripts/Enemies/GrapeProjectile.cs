using System.Collections;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadow;
    [SerializeField] private GameObject splatterPrefab;
    private void Start()
    {
        GameObject grapeShadow = Instantiate(grapeProjectileShadow, transform.position + new Vector3(0,-0.3f,0),Quaternion.identity);
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;

        Vector3 playerPosition = PlayerController.Instance.transform.position;

        StartCoroutine(MoveGrapeRoutine(grapeShadow,grapeShadowStartPosition,playerPosition));
        StartCoroutine(ProjectileCurveRoutine(transform.position,playerPosition));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;
        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / duration;
            float heightTime = animationCurve.Evaluate(linearTime);
            float height = Mathf.Lerp(0,heightY, heightTime);
            transform.position = Vector2.Lerp(startPosition,endPosition,linearTime) + new Vector2(0f, height);
            yield return null;
        }
        Instantiate(splatterPrefab, transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveGrapeRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;
        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / duration;
            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearTime);
            yield return null;
        }
        Destroy(grapeShadow);
    }
}
