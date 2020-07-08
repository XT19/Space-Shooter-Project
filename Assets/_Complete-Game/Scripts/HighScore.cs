using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HighScores : IComparable<HighScores>
{

    public string Name { get; set; }

    public int Score { get; set; }

    public int Wave { get; set; }

    public DateTime Date { get; set; }

    public int Id { get; set; }

    public HighScores(int id,string name,int score,int wave, DateTime date)
    {
        this.Id = id;
       this.Name = name;
       this.Score = score;
       this.Wave = wave;
       this.Date = date;
    }

    public int CompareTo(HighScores other)
    {
        //first > second return -1
        //first < second return 1
        //first == second return 0
        if (other.Score < this.Score)
        {
            return -1;
        }
        else if (other.Score > this.Score)
        {
            return 1;
        }
        else if (other.Date < this.Date)
        {
            return -1;
        }
        else if (other.Date > this.Date)
        {
            return 1;
        }
        return 0;
    }
}
