using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public SlingShooter slingShooter;
    public TrailController trailController;
    public List<Bird> birds;
    public List<Enemy> enemies;
    public BoxCollider2D tapCollider;
    private Bird _shotBird;

    private bool isGameEnded = false;

    [Header("Initial Setup")]
    public Transform _prepArea;
    public float _birdSpacing = 1f;
    public float blackBirdYOffset = -0.15f;

    private void Start()
    {
        for (int i=0; i< birds.Count; i++)
        {
            float xPosition = _prepArea.position.x - _birdSpacing * i;
            float yPosition = _prepArea.position.y;

            if (birds[i].gameObject.name == "BlackBird")
                if (birds[i] == null)
                    return;

                yPosition = _prepArea.position.y + blackBirdYOffset;

            birds[i].gameObject.transform.position = new Vector2(xPosition, yPosition);
        }

        for (int i = 0; i < birds.Count; i++)
        {
            birds[i].OnBirdDestroyed += ChangeBird;
            birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < enemies.Count; i++)
            enemies[i].OnEnemyDestroyed += CheckGameEnd;

        tapCollider.enabled = false;
        slingShooter.InitiatedBird(birds[0]);
        _shotBird = birds[0];
    }

    public void ChangeBird()
    {
        tapCollider.enabled = false;

        if (isGameEnded)
            return;

        birds.RemoveAt(0);

        for (int i = 1; i < birds.Count; i++)
        {
            if (birds[i] == null)
                return;

            float xPosition = _prepArea.position.x - _birdSpacing * i;
            float yPosition = _prepArea.position.y;

            if (birds[i].gameObject.name == "BlackBird")
                yPosition = _prepArea.position.y + blackBirdYOffset;

            birds[i].gameObject.transform.position = new Vector2(xPosition, yPosition);
        }

        if (birds.Count > 0)
        {
            slingShooter.InitiatedBird(birds[0]);
            _shotBird = birds[0];
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < enemies.Count; i++) 
            if (enemies[i].gameObject == destroyedEnemy)
            {
                enemies.RemoveAt(i);
                break;
            }

        if (enemies.Count == 0)
        {
            isGameEnded = true;
        }
    }

    public void AssignTrail(Bird bird)
    {
        trailController.SetBird(bird);
        StartCoroutine(trailController.SpawnTrail());

        tapCollider.enabled = true;
    }

    private void OnMouseUp()
    {
        if (_shotBird != null)
            _shotBird.OnTap();
    }
}
