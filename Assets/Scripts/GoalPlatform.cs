using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return null; //todo: Insert fancy effect!
        ResetButton.LastLevelLoadReason = ResetButton.LastLevelLoadReason = ResetButton.LevelLoadReason.Finish;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.DrawLine(collision.collider.transform.position, collision.otherCollider.transform.position, Color.red, 10);
    }
}
