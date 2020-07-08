using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount, EnnemiesPerWave, score, SaveScores;
    public float spawnWait, startWait, waveWait, hasardSpawnDifficulty;
    public int waveCount = 1;
    private float gameOverWait = 2;
    public GameObject pauseMenu, scoreField, saveButton;
    private string connectionString;

    private List<HighScores> highScores = new List<HighScores>();



    public Text scoreText, FinalScore, restartText, quitText, waveText,gameOverText,nameScore,warningText;
    public AudioClip wastedSound;

    private bool gameOver, restart;



    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/_Complete-Game/DataBase/HighScoreDB.sqlite";
        scoreField.active = false;
        saveButton.active = false;
        pauseMenu.active = false;
        gameOver = false;
        restart = false;
        warningText.text = "";
        FinalScore.text = "";
        restartText.text = "";
        quitText.text = "";
        waveText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

  

    void Update()
    { 

            if (restart)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[UnityEngine.Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            spawnWait = spawnWait - hasardSpawnDifficulty;
            hazardCount = hazardCount + EnnemiesPerWave;
            waveCount++;
            waveText.text = "Wave : " + waveCount.ToString();
            yield return new WaitForSeconds(waveWait);
            waveText.text = "";


            if (gameOver)
            {
                

                restartText.text = "Press 'F2' for Restart";

                quitText.text = "Press 'F5' to go back to menu";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = wastedSound;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().volume = 0.2f;
        GetComponent<AudioSource>().Play();

        gameOverText.text = "GAME OVER";
        FinalScore.text = "Final Score : " + score.ToString() + "\n" + "Wave : " + waveCount.ToString();
        gameOver = true;
        scoreField.active = true;
        saveButton.active = true;

    }









    public void EnterName()
    {
        if (nameScore.text != string.Empty)
        {
            connectionString = "URI=file:" + Application.dataPath + "/HighScoreDB.sqlite";
            InsertScore(nameScore.text, score, waveCount);
            nameScore.text = "";
            saveButton.active = false;
            warningText.color = new Color(255, 255, 0);
            warningText.text = "Score Saved !";
        }
        else
        {
            warningText.text = "Put a name !";
        }
    }



    public void InsertScore(string newName, int newScore, int newWave)
    {
        GetScores();
        int hsScore = highScores.Count;

        if (highScores.Count > 0)
        {
            HighScores lowestScore = highScores[highScores.Count - 1];
            if ((lowestScore != null) && (SaveScores > 0) && (highScores.Count >= SaveScores) && (newScore > lowestScore.Score))
            {
                DeleteScore(lowestScore.Id);
                hsScore--;
            }
        }

        if (hsScore < SaveScores)
        {
            using (IDbConnection dbConnection = new SqliteConnection(connectionString))
            {

                dbConnection.Open();

                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    string sqlQuery = String.Format("INSERT INTO HighScores(name,score,wave) VALUES (\"{0}\",{1},{2})", newName, newScore, newWave);

                    dbCmd.CommandText = sqlQuery;
                    dbCmd.ExecuteScalar();
                    dbConnection.Close();
                }
            }
        }
    }

    private void GetScores()
    {
        highScores.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {

            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "select * from highscores";
                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        highScores.Add(new HighScores(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetDateTime(4)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        highScores.Sort();
    }

    private void DeleteScore(int id)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {

            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("delete from highscores where id =\'{0}\'", id);

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }
    

}