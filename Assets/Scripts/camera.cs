using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour
{
    [SerializeField] private GameObject PlayerOne;
    [SerializeField] private GameObject PlayerTwo;

    [SerializeField] private LayerMask playerLayers;

    private float minZoom = 5f;
    private float maxZoom = 10f;
    private float zoomSpeed = 5f;

    private float minX = -10f;
    private float maxX = 10f;
    private float minY = -5f;
    private float maxY = 5f;

    public bool camFollow;

    private void Update()
    {
        if (camFollow)
        {
            Vector3 middlePos = new Vector3((PlayerOne.transform.position.x + PlayerTwo.transform.position.x) / 2, (PlayerOne.transform.position.y + PlayerTwo.transform.position.y) / 2, -10);
            middlePos.x = Mathf.Clamp(middlePos.x, minX, maxX);
            middlePos.y = Mathf.Clamp(middlePos.y, minY, maxY);
            this.gameObject.transform.position = middlePos;
            float distance = Vector2.Distance(PlayerOne.transform.position, PlayerTwo.transform.position);
            float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / maxZoom);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        }
    }

    public void Shake(float duration, AnimationCurve curve)
    {
        StartCoroutine(Shaker(duration, curve));
    }

    public IEnumerator Shaker(float duration, AnimationCurve curve)
    {
        Debug.Log("cam.shake()");
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Vector3 startPos = new Vector3(transform.position.x, transform.position.y, -10);
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }
    }
}
