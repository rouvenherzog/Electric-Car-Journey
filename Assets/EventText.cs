using System.Collections;
using TMPro;
using UnityEngine;

public class EventText : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public void Start() {
        (transform as RectTransform).sizeDelta = Vector2.zero;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        yield return ChangeHeightTo(50);
        yield return new WaitForSecondsRealtime(2f);
        yield return ChangeHeightTo(0);
    }

    private IEnumerator ChangeHeightTo(int targetHeight) {
        RectTransform rectTransform = (RectTransform) transform;
        float changeProgress = 0;
        Vector2 origin = Vector2.up * rectTransform.sizeDelta.y;
        Vector2 destination = Vector2.up * targetHeight;
        float stepDuration = 1 / 24f;
        float animationDuration = 0.2f;
        float stepDelta = stepDuration / animationDuration;
        while(changeProgress < 1) {
            rectTransform.sizeDelta =
                Vector2.Lerp(origin, destination, changeProgress);
            yield return new WaitForSecondsRealtime(stepDuration);
            changeProgress += stepDelta;
        }
        rectTransform.sizeDelta = destination;
    }
}
