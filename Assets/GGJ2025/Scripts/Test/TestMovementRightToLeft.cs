using UnityEngine;

public class TestMovementRightToLeft : MonoBehaviour
{

    [SerializeField]
    private float velocity = 1;
    private Transform _transform;

    void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate() {
        _transform.position += Vector3.left * velocity * Time.fixedDeltaTime;
    }
}
