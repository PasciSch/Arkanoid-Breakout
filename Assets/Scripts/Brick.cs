﻿using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    public int Hitpoints = 1;

    public static event Action<Brick> OnBrickDestruction;
    public ParticleSystem DestroyEffect;

    private void Awake() {
        this.sr = this.GetComponent<SpriteRenderer>();
        this.boxCollider = this.GetComponent<BoxCollider2D>();
        Ball.OnLightningBallEnable += OnLightningBallEnable;
        Ball.OnLightningBallDisable += OnLightningBallDisable;
    }

    private void OnLightningBallEnable(Ball ball)
    {
        if(this != null)
        {
            this.boxCollider.isTrigger = true;
        }
    }
    private void OnLightningBallDisable(Ball ball)
    {
        if(this != null)
        {
            this.boxCollider.isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger) 
    {
        if (trigger.gameObject.tag == "Ball")
        {
            Ball ball = trigger.gameObject.GetComponent<Ball>();
            ApplyCollisionLigic(ball);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Ball")
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            ApplyCollisionLigic(ball);
        }
    }

    private void ApplyCollisionLigic(Ball ball)
    {
        this.Hitpoints--;

        if (this.Hitpoints <= 0 || (ball != null && ball.isLightningBall))
        {
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            OnBrickDestroy();
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = BricksManager.Instance.Sprites[this.Hitpoints - 1];
        }
    }

    private void OnBrickDestroy()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float deBuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alredySpawned = false;

        if (buffSpawnChance <= CollectableManager.Instance.BuffChance)
        {
            alredySpawned = true;
            CollectableManager.Instance.CurrentCollectables.Add(this.SpawnCollectable(true));
        }
        
        if(deBuffSpawnChance <= CollectableManager.Instance.DebuffChance && !alredySpawned)
        {
            CollectableManager.Instance.CurrentCollectables.Add(this.SpawnCollectable(false));
        }
    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;

        if (isBuff)
        {
            collection = CollectableManager.Instance.AvailableBuffs;
        }
        else
        {
            collection = CollectableManager.Instance.AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectable;
        return newCollectable;
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    internal void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.Hitpoints = hitpoints;
    }

    private void OnDisable() 
    {
        Ball.OnLightningBallEnable -= OnLightningBallEnable;
        Ball.OnLightningBallDisable -= OnLightningBallDisable;
    }
}
