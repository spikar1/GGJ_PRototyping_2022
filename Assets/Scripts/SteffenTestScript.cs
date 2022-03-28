using UnityEngine;

public class SteffenTestScript : MonoBehaviour
{
    private void Update()
    {

    }
}


/*
public class SteffenTestScript : MonoBehaviour
{
    [Tooltip("This value has meaning and functionality within the game")]
    public float someFloat;

    [Range(-5,5)]
    public float aRangedValue;

    [Header("This is a title within the inspector")]
    [Multiline(3)]
    public string multilineString;

    [Space(20)]
    public int intSpacedFromTheRest = 4;
}



/*
public class SteffenTestScript : MonoBehaviour
{
    public float duration = 0;
    public Vector3 from, to;
    public Vector3 fromRay, direction;
    public Vector3 center, size;
    public Vector3 centerSphere;
    public float radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(from, to);
        Gizmos.DrawRay(fromRay, direction);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(center, size);
        Gizmos.DrawWireSphere(centerSphere, radius);
    }
}

/*
public class SteffenTestScript : MonoBehaviour
{
    public float duration = 0;
    public Vector3 start, end;
    public Vector3 rayStart, direction;

    void Update()
    {
        Debug.DrawLine(start, end, Color.red, duration);
        Debug.DrawRay(rayStart, direction, Color.magenta, duration);
        Debug.Log("Some debug message", this);
        Debug.LogError("Some message about whats wrong", this);
    }
}
*/