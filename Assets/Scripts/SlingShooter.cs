using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D Collider;
    public LineRenderer trajectory;

    private Vector2 _startPos;
    private Bird _bird;

    [SerializeField] private float _radius = 0.75f;
    private float _throwSpeed = 30f;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void OnMouseUp()
    {
        Collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(velocity, distance, _throwSpeed);

        gameObject.transform.position = _startPos;
        trajectory.enabled = false;
    }

    private void OnMouseDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - _startPos;
        if (direction.sqrMagnitude>_radius)
            direction = direction.normalized * _radius;
        transform.position = _startPos + direction;

        float distance = Vector2.Distance(_startPos, transform.position);

        if (!trajectory.enabled)
            trajectory.enabled = true;

        DisplayTrajectory(distance);
    }

    public void InitiatedBird(Bird bird)
    {
        Debug.Log("Initiated");
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        Collider.enabled = true;
    }

    void DisplayTrajectory( float distance)
    {
        if (_bird == null)
            return;

        Vector2 velocity = _startPos - (Vector2)transform.position;
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        segments[0] = transform.position;

        Vector2 segVelocity = velocity * _throwSpeed * distance;

        for (int i=1; i < segmentCount; i++)
        {
            float elapsedTIme = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapsedTIme + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTIme, 2);
        }

        trajectory.positionCount = segmentCount;

        for (int i = 0; i < segmentCount; i++)
            trajectory.SetPosition(i, segments[i]);
    }
}
