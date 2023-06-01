using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System;
using UnityEngine;
//using System.Math;


public class Predator : MonoBehaviour
{
    public float movingSpeed = 4f;
    public float attackingSpeeds;

    public int nPredatorTypes = 3;

    bool colorShuffleActive = false; //Checks if first set has been completed

    [Tooltip("Turn on to have predator colors shuffle after each set")]
    public bool shuffleColorsAfterNTrials = true;
    [Tooltip("Turn on to activate small distribution waves after each set")]
    public bool changeToSmallDistributionAfterNTrials = true;
    [Tooltip("Turn on to activate overlapping distributions")]
    public bool shiftDistributionEachSet = false;
    [Tooltip("Diplays values for testing")]
    public bool debugActive = false;
    public Color[] colors = { Color.red, Color.blue, Color.yellow };

    //unifrom distribution
    public double[] uniform_means = { 20, 40, 60 };
    public double[] uniform_ranges = { 10, 10, 10 };
    //normal distribution
    public double[] normal_means = { 20, 40, 60 };
    public double[] normal_standard_divisions = { 10, 10, 10 };
    //probability for generating a outlier (+3 ~ +6 standard division) for normal distribution
    public double[] normal_outlier_probabilities = { 0.2, 0.2, 0.2 };
    //mean for exponential distribution
    public double[] exp_mean = { 20, 40, 60 };
    [Tooltip("When shiftDistributionEachSet is enabled, each distribution will be shifted by these amounts respectively.")]
    public double[] setShift = { 0, 5, 10 };

    private Rigidbody2D body;
    bool begin = false; //When true, movement starts (controlled by the Countdown coroutine)
    private int predatorType;
    private bool attacking = false;
    private System.Random rand = new System.Random();
    private double attackingDistance;
    private const int MAX_ATTACKING_DISTANT = 80;
    ScoreDisplay scoreDisplay;
    SpriteRenderer sprite;

    private const int N_DISTRIBUTION_TYPES = 3;
    //Type of distribution for generating attacking distance
    //0: uniform, 1: normal, 2: exponential 
    private int distributionType = N_DISTRIBUTION_TYPES - 1;

    UnityEngine.Vector2 initialPosition = new UnityEngine.Vector2(-29.5f, 1f);

    /**
    reset the position, state, type, and attacking distant
    */

    public void reset()
    {
        body.velocity = new UnityEngine.Vector2(0f, 0f);
        transform.position = initialPosition;
        attacking = false;
        predatorType = rand.Next(nPredatorTypes);
        sprite.color = colors[predatorType];
        DataManager.RecordingTrial.PredatorColor = sprite.color;
		attackingDistance = generate_attacking_distance(shiftDistributionEachSet);
        DataManager.RecordingTrial.PredatorAttackingDistance = (float)attackingDistance;
        attackingSpeeds = (float)attackingDistance / 2 > 40 ? (float)attackingDistance / 2 : 40;
        if (debugActive)
        {
            print("Trial Count: " + scoreDisplay.GetTrialCount() + " PredatorType: " + predatorType + " Distance: " + UnityEngine.Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) + "Attacking from: " + attackingDistance);
        }
    }

    /**
    shuffle the colors corresponding to different types of predators. To turn on shuffle, click the shuffleColors check box
    */
    public void shuffle_color()
    {
        if (colorShuffleActive)
        {
            for (int i = 0; i < nPredatorTypes; ++i)
            {
                int j = rand.Next(nPredatorTypes);
                Color temp = colors[i];
                colors[i] = colors[j];
                colors[j] = temp;
            }
        }
        else return;
    }

    /**
    change the distribution used to generate attacking distant
    */
    public void change_distribution_type()
    {
        distributionType = (++distributionType) % N_DISTRIBUTION_TYPES;
    }
    /**
    @return the attacking distant for current predator
    */
    public double get_attacking_distance()
    {
        return attackingDistance;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreDisplay = FindObjectOfType<ScoreDisplay>();
        shuffle_color();
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1);
        distributionType = 0;
        reset();
        begin = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (begin)
        {
            move();
        }
    }

    void move()
    {
        if (attacking)
        {   //attacking
            UnityEngine.Vector2 movement = new UnityEngine.Vector2(attackingSpeeds, body.velocity.y);
            body.velocity = movement;
        }
        else
        {
            UnityEngine.Vector2 movement = new UnityEngine.Vector2(movingSpeed, body.velocity.y);
            body.velocity = movement;

            Transform player = GameObject.FindWithTag("Player").transform;
            UnityEngine.Vector2 playerPosition = player.position;

            if (UnityEngine.Vector2.Distance(transform.position, playerPosition) < attackingDistance)
            {   //reach attacking distance
                attacking = true;
            }
        }
    }

    double random_normal(int predatorType)
    {   //Box-Muller transform
        if (debugActive)
        {
            print("Distribution Type: Random Normal");
        }
        if (scoreDisplay.GetTrialCount() < ScoreDisplay.nTrialsPerSet + 1 || changeToSmallDistributionAfterNTrials == false)
        {
            return normal_means[predatorType] + normal_standard_divisions[predatorType] *
            (Math.Sqrt(-2 * Math.Log(1 - rand.NextDouble())) * Math.Cos(2 * Math.PI * rand.NextDouble()));
        }
        else
        {
            int rndDist = GetRandomWave();
            if (rndDist == 0)
            {
                return (normal_means[predatorType] - (20 / 3) + normal_standard_divisions[predatorType] / 3 *
                (Math.Sqrt(-2 * Math.Log(1 - rand.NextDouble())) * Math.Cos(2 * Math.PI * rand.NextDouble())));
            }
            else if (rndDist == 1)
            {
                return normal_means[predatorType] + normal_standard_divisions[predatorType] / 3 *
                (Math.Sqrt(-2 * Math.Log(1 - rand.NextDouble())) * Math.Cos(2 * Math.PI * rand.NextDouble()));
            }
            else
            {
                return normal_means[predatorType] + (20 / 3) + normal_standard_divisions[predatorType] / 3 *
                (Math.Sqrt(-2 * Math.Log(1 - rand.NextDouble())) * Math.Cos(2 * Math.PI * rand.NextDouble()));
            }
        }
    }

    double random_uniform(int predatorType, double amountToShift)
    {
        if (scoreDisplay.GetTrialCount() < ScoreDisplay.nTrialsPerSet + 1 || changeToSmallDistributionAfterNTrials == false)
        {
            return uniform_means[predatorType] + (rand.NextDouble() - 1 / 2) * uniform_ranges[predatorType] + amountToShift;
        }
        else
        {
            int rndDist = GetRandomWave();
            if (rndDist == 0)
                return (uniform_means[predatorType] - (20 / 3) + (rand.NextDouble() - 1 / 2) * uniform_ranges[predatorType] / 3);
            else if (rndDist == 1)
            {
                return (uniform_means[predatorType] + (rand.NextDouble() - 1 / 2) * uniform_ranges[predatorType] / 3);
            }
            else
            {
                return (uniform_means[predatorType] + (20 / 3) + (rand.NextDouble() - 1 / 2) * uniform_ranges[predatorType] / 3);
            }

        }
    }

    double random_normal_with_outlier(int predatorType)
    {
        if (rand.NextDouble() < normal_outlier_probabilities[predatorType])
        {
            return normal_means[predatorType] + normal_standard_divisions[predatorType] * 3 +
            rand.NextDouble() * normal_standard_divisions[predatorType] * 3;
        }
        else
        {
            return random_normal(predatorType);
        }
    }

    double random_exp(int predatorType)
    {
        if (scoreDisplay.GetTrialCount() < ScoreDisplay.nTrialsPerSet + 1 || changeToSmallDistributionAfterNTrials == false)
        {
            return -Math.Log(1 - rand.NextDouble()) * exp_mean[predatorType];
        }
        else
        {
            int rndDist = GetRandomWave();
            if (rndDist == 0)
            {
                return -Math.Log(1 - rand.NextDouble()) * exp_mean[predatorType] / 3 - (20 / 3);
            }
            else if (rndDist == 1)
            {
                return -Math.Log(1 - rand.NextDouble()) * exp_mean[predatorType] / 3;
            }
            else
            {
                return -Math.Log(1 - rand.NextDouble()) * exp_mean[predatorType] / 3 + (20 / 3);
            }
        }
    }

    double generate_attacking_distance(bool overlappingDistribution)
    {
        if (!overlappingDistribution)
        {
            switch (distributionType)
            {
                case 0: return random_uniform(predatorType, setShift[0]);

                case 1: return random_normal(predatorType);

                case 2: return random_exp(predatorType);

                //case 3: return random_normal_with_outlier(predatorType);

                default: return 0;
            }
        }
        else
        {
            switch (distributionType)
            {
                case 0: return random_uniform(predatorType, setShift[0]);

                case 1: return random_uniform(predatorType, setShift[1]);

                case 2: return random_uniform(predatorType, setShift[2]);

                default: return 0;
            }
        }
    }
    private int GetRandomWave()
    {
        return UnityEngine.Random.Range(0, 3);
    }
    public bool GetColorShuffleActive()
    {
        return colorShuffleActive;
    }
    public void SetColorShuffleActive(bool value)
    {
        colorShuffleActive = value;
    }
}
