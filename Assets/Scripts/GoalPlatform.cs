using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPlatform : MonoBehaviour
{
    [SerializeField]
    string levelString = "INSERT LEVEL NAME";



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            StartCoroutine(LoadLevel(levelString));
    }

    IEnumerator LoadLevel(string levelName)
    {
        yield return null; //todo: Insert fancy effect!
        SceneManager.LoadScene(levelName);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.DrawLine(collision.collider.transform.position, collision.otherCollider.transform.position, Color.red, 10);
    }
}
