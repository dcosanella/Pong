﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool m_paused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_paused && Input.GetKeyDown(KeyCode.Space))
        {
            m_paused = true;
            Time.timeScale = 0;
        }
        else if (m_paused && Input.GetKeyDown(KeyCode.Space))
        {
            m_paused = false;
            Time.timeScale = 1;
        }
    }
}
