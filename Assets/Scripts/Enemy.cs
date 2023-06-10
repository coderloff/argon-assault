using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject hitVFX;
    [SerializeField] Transform parent;
    [SerializeField] int scorePerHit = 15;

    [SerializeField] int hitPoints = 2;

    ScoreBoard scoreBoard;

    private void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        ProcessDamage();
    }

    void ProcessHit()
    {
        scoreBoard.IncreaseScore(scorePerHit);
    }

    void ProcessDamage()
    {
        hitPoints--;
        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
        if (hitPoints < 1)
        {
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
        Destroy(gameObject);
    }
}
