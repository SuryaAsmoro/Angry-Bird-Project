using UnityEngine;

public class BlackBird : Bird
{
    public float _blastForce = 50f;
    public float _blastRadius = 5f;

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _blastRadius);

        foreach (Collider2D hitCollider in colliders)
        {
            Rigidbody2D rigidBody = hitCollider.GetComponent<Rigidbody2D>();
            if (hitCollider!= null)
            {
                Vector2 direction = hitCollider.gameObject.transform.position - transform.position;
                float magnitude = _blastForce / (Mathf.Abs(Vector2.Distance(transform.position, hitCollider.gameObject.transform.position)));

                rigidBody.AddForce(direction*magnitude);
                Debug.Log("Direction= " + direction);
            }

            Debug.Log("Explode");
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Enemy")
            Explode();
    }
}
