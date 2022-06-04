using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using System.Text;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    private List<KeyFrame> keyFrames = new List<KeyFrame>(15);

    public GameObject player;

    public float TimeInSeconds;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI LevelText;
    public bool isStatic = false;
    public MazeGenerator mazeGenerator;

    public float spotlightRadius = 5f;

    public int currentLevel = 1;
    public int numberOfLevels;

    public float difficultyRating;

    private int _counter;
    public int Counter
    {
        get => _counter;
        set
        {
            _counter = value;
            keyFrames.Add(new KeyFrame(value, TimeInSeconds));
        }
    }

    public string ToCSV()
    {
        var sb = new StringBuilder("Time Taken (secs),Difficulty Rating");
        foreach (var frame in keyFrames)
        {
            sb.Append('\n').Append(frame.Time.ToString()).Append(',').Append(frame.Value.ToString());
        }

        return sb.ToString();
    }

    public void SaveToFile()
    {
        // Use the CSV generation from before
        var content = ToCSV();

        // The target file path e.g.
#if UNITY_EDITOR
        var folder = Application.streamingAssetsPath;

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
#else
      var folder = Application.streamingAssetsPath;
#endif

        var filePath = "";

        if (isStatic)
        {
            filePath = Path.Combine(folder, "static_results.csv");

        }
        else
        {
            filePath = Path.Combine(folder, "dynamic_results.csv");
        }

        using (var writer = new StreamWriter(filePath, false))
        {
            writer.Write(content);
        }

        Debug.Log($"CSV file written to \"{filePath}\"");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public void Start()
    {
        Counter = 0;
        
        if (PlayerPrefs.GetInt("DynamicModeEnabled") == 0)
        {
            isStatic = true;
            Debug.Log("Dynamic mode OFF");
        }
        else
        {
            isStatic = false;
            Debug.Log("Dynamic mode ON");
        }

        StartCoroutine(IncrementTimer());

        if (currentLevel == 1)
        {
            mazeGenerator.GenerateMaze(15, 15);
        }
        LevelText.text = "Level: " + currentLevel.ToString();
    }

    IEnumerator IncrementTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            //every 60 seconds turn up light
            if (TimeInSeconds > 0 && TimeInSeconds % 60 == 0 && spotlightRadius < 10)
            {
                spotlightRadius += 2;
            }
            player.GetComponentInChildren<Light2D>().pointLightOuterRadius = spotlightRadius;
            TimeInSeconds++;
            float minutes = Mathf.FloorToInt(TimeInSeconds / 60);
            float seconds = Mathf.FloorToInt(TimeInSeconds % 60);
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void AdjustDifficulty()
    {
        difficultyRating = 0;
        currentLevel++;
        LevelText.text = "Level: " + currentLevel.ToString();

        if (isStatic)
        {
            difficultyRating = (10 + currentLevel);
            // every two levels, reduce spotlight strength by 2
            if ((currentLevel % 2) == 0)
            {
                spotlightRadius -= 2;
            }
        }
        else
        {
            Debug.Log("10/" + TimeInSeconds + "/180");
            difficultyRating = (10 / (TimeInSeconds / 50));
        }

        if (difficultyRating < 10)
        {
            difficultyRating = 10;
        }

        else if (difficultyRating > 20)
        {
            difficultyRating = 20;
        }

        spotlightRadius = (TimeInSeconds) / 15;
        if (spotlightRadius < 2)
        {
            spotlightRadius = 2;
        }

        Counter = (int)difficultyRating;
        //Debug.Log(ToCSV());
        TimeInSeconds = 0;
        mazeGenerator.GenerateMaze((int)difficultyRating, (int)difficultyRating);
        Debug.Log("Level " + currentLevel.ToString() + " of " + numberOfLevels + " Reached: Adjusting difficulty...");
        Debug.Log("New difficulty rating: " + difficultyRating.ToString() + " tiles, Spotlight Size: " + spotlightRadius.ToString());

    }

    public void Update()
    {
        // timeout in case player gets stuck
        if(TimeInSeconds == 180 && !isStatic)
        {
            AdjustDifficulty();
        }

        if (currentLevel == numberOfLevels)
        {
            SaveToFile();
            SceneManager.LoadScene(0);
        }

        //debug function: end game when K is pressed
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    SaveToFile();
        //    SceneManager.LoadScene(0);
        //}
    }
}
