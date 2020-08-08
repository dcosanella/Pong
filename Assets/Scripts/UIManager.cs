using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text m_playerScore;
    [SerializeField]
    private Text m_cpuScore;

    // Start is called before the first frame update
    void Start()
    {
        if (m_playerScore == null || m_cpuScore == null)
        {
            throw new System.NullReferenceException();
        }
        
        string initialScore = "0";
        m_cpuScore.text = initialScore;
        m_playerScore.text = initialScore;
    }

    public void UpdateScore(string paddle, int score)
    {
        if (paddle == "Player")
        {
            m_playerScore.text = score.ToString();
            return;
        }

        m_cpuScore.text = score.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
