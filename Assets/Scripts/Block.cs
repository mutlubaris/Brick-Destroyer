using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparkles;
    [SerializeField] int timesHit;
    [SerializeField] Sprite[] hitSprites;

    Level level;
    GameSession gamestatus;


    private void Start()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBreakableBlocks();
        }
        gamestatus = FindObjectOfType<GameSession>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            timesHit++;
            int maxHits = hitSprites.Length + 1;
            if (timesHit >= maxHits)
            {
                DestroyBlock();
            }
            else
            {
                ShowNextHitSprite();
            }
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if(hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block sprite is missing from array " + gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        gamestatus.AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerSparkles();
    }

    private void TriggerSparkles()
    {
        GameObject sparkles = Instantiate(blockSparkles, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
