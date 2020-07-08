using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour
{

    public GameObject gameObjectname;
    public GameObject gameObjectScore;
    public GameObject gameObjectWave;
    public GameObject gameObjectRank;

    public void setScore(string name,string score,string wave,string rank)
    {
        this.gameObjectname.GetComponent<Text>().text = name;
        this.gameObjectScore.GetComponent<Text>().text = score;
        this.gameObjectWave.GetComponent<Text>().text = wave;
        this.gameObjectRank.GetComponent<Text>().text = rank;
    }
}
