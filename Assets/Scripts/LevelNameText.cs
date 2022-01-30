using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNameText : MonoBehaviour
{
    float waitTime = 1;
    float animationLength = 1;

    Vector3 offset => Vector2.up * 2;

    

    IEnumerator Start()
    {
        GetComponent<TMPro.TMP_Text>().text = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        var endPos = transform.position + offset;
        var startPos = transform.position;
        yield return new WaitForSecondsRealtime(waitTime);
        for (float t = 0f; t < animationLength; t += Time.unscaledDeltaTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / animationLength);
            yield return null;
        }
    }

}
