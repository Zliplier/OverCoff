using UnityEngine;

public class FloatingNote : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float floatSpeed = 2f;
    public float floatHeight = 20f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}
