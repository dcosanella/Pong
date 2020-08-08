using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField]
    private Paddle m_aiPaddle;

    private Ball m_ball;

    // Start is called before the first frame update
    void Start()
    {
        if (m_aiPaddle == null)
        {
            throw new System.NullReferenceException();
        }

        m_ball = GameObject.Find("Ball").GetComponent<Ball>();
        if (m_ball == null)
        {
            throw new System.NullReferenceException();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.Translate(GetBallPosition());
    }

    private Vector3 GetBallPosition()
    {
        if (m_ball.transform.position.y < 0)
        {
            return Vector3.down;
        }

        return Vector3.up;
    }
}
