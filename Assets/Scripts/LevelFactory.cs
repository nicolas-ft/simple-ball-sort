﻿using UnityEngine;

public class LevelFactory : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int bottlesQty;
    [SerializeField] private int ballCount;
    [SerializeField] private float bottleMargin;
    [SerializeField] private int bottleRowQty;
    [Header("Prefabs")]
    [SerializeField] private GameObject bottlePrefab = null;
    [SerializeField] private GameObject ballPrefab = null;

    public Bottle[] generateLevel(GameManager gameManager)
    {
        Bottle[] bottles = instantiateBottles(gameManager);
        Ball[] ballList = createBallList();

        shuffleBalls(ref ballList);
        populateBottles(ref bottles, ballList);

        return bottles;
    }

    private Bottle[] instantiateBottles(GameManager gameManager)
    {
        Sprite bottleSprite = Resources.Load<Sprite>("Bottle_1");
        Bottle[] bottles = new Bottle[bottlesQty + 2];

        // Populate array with new Bottles
        for (int i = 0; i < bottles.Length; i++)
        {
            GameObject bottleObj = Instantiate(bottlePrefab);

            float xPosi = i % bottleRowQty * bottleMargin;
            float yPosi = (i / bottleRowQty) * -(bottlesQty + 1);

            bottleObj.transform.position = new Vector3(xPosi, yPosi, 0);

            Bottle newBottle = bottleObj.GetComponent<Bottle>();
            newBottle.initialize(ballCount, gameManager);
            newBottle.updateBottle(bottleSprite);
            bottles[i] = newBottle;
        }

        return bottles;
    }

    private Ball[] createBallList()
    {
        Ball[] ballList = new Ball[ballCount * bottlesQty];

        // Populate array with balls in order
        for (int i = 0; i < bottlesQty; i++)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            for (int j = 0; j < ballCount; j++)
            {
                Ball newBall = Instantiate(ballPrefab).GetComponent<Ball>();
                newBall.initialize(i, randomColor);
                ballList[i * ballCount + j] = newBall;
            }
        }

        return ballList;
    }

    private void shuffleBalls(ref Ball[] ballList)
    {
        int randomIndex;
        for (int i = ballList.Length - 1; i >= 0; i--)
        {
            randomIndex = Random.Range(0, i);
            if (randomIndex != i)
            {
                Ball aux = ballList[randomIndex];
                ballList[randomIndex] = ballList[i];
                ballList[i] = aux;
            }
        }
    }

    private void populateBottles(ref Bottle[] bottles, Ball[] ballList)
    {
        for (int i = 0; i < bottlesQty; i++)
        {
            for (int j = 0; j < ballCount; j++)
                bottles[i].forcePush(ballList[i * ballCount + j]);
        }
    }
}
