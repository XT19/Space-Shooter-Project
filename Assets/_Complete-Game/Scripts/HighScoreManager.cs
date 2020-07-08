using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour
{

    private String connectionString;
    
    private List<HighScores> highScores = new List<HighScores>();

    public GameObject scorePrefab;

    public Transform scoreParent;



    public int SaveScores;

    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/HighScoreDB.sqlite";

        CreateTable();
        DeleteExtraScore();
        ShowScores();


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
                        highScores.Add(new HighScores(reader.GetInt32(0),reader.GetString(1) ,reader.GetInt32(2), reader.GetInt32(3),reader.GetDateTime(4)));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        highScores.Sort();
    }

    private void CreateTable()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {

            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("Create Table if not exists HighScores(id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE ,name TEXT,score INTEGER,wave INTEGER,Date DATETIME DEFAULT CURRENT_DATE)");
                dbCmd.CommandText = sqlQuery;

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
                
                }
            }
        }

    public void DeleteAllScore()
    {
        GetScores();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {

            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                for (int i = 0; i < highScores.Count; i++)
                {
                    string sqlQuery = String.Format("delete from highscores where id =\'{0}\'", highScores[i].Id);

                    dbCmd.CommandText = sqlQuery;
                    dbCmd.ExecuteScalar();
                    
                }
                dbConnection.Close();
                
            }
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    private void DeleteExtraScore() {

        GetScores();

        if (SaveScores < highScores.Count) {

            int DeletCount = highScores.Count - SaveScores;
            highScores.Reverse();

            using (IDbConnection dbConnection = new SqliteConnection(connectionString))
            {

                dbConnection.Open();

                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    for (int i = 0; i < DeletCount; i++) {
                        string sqlQuery = String.Format("delete from highscores where id =\'{0}\'", highScores[i].Id);
                        dbCmd.CommandText = sqlQuery;
                        dbCmd.ExecuteScalar();

                    }
                    dbConnection.Close();
                }
            }

        }
    }


    

    public void ShowScores()
        
    {
        GetScores();
        for (int i = 0; i < highScores.Count; i++)
        {
    
            GameObject tempObjet = Instantiate(scorePrefab);

            HighScores tempScore = highScores[i];

            tempObjet.GetComponent<HighScoreScript>().setScore(tempScore.Name, tempScore.Score.ToString(), tempScore.Wave.ToString(),"#"+(i + 1).ToString());

            tempObjet.transform.SetParent(scoreParent);

        }
    }


    
}
    



