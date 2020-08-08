using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField]
    private Paddle m_player;
    [SerializeField]
    private Paddle m_ai;
    [SerializeField]
    private Ball m_ball;

    private float m_playerSpeed = 20;
    private float m_aiSpeed = 25;

    // Start is called before the first frame update
    void Start()
    {
        if (m_player == null || m_ai == null)
        {
            Debug.LogException(new System.NullReferenceException());
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
        CalculatePlayerMovement();
        CalculateAIMovement();
    }

    private void CalculatePlayerMovement()
    {
        bool collidingWithTop = m_player.CollidingWithTop();
        bool collidingWithBottom = m_player.CollidingWithBottom();

        float input = Input.GetAxis("Vertical");

        if (!collidingWithTop && !collidingWithBottom
            || collidingWithTop && input < 0
            || collidingWithBottom && input > 0)
        {
            m_player.transform.Translate(new Vector3(0, input * m_playerSpeed * Time.deltaTime, 0));
        }
    }

    private void CalculateAIMovement()
    {
        float direction = m_ball.GetBallDirection();
        bool collidingWithTop = m_ai.CollidingWithTop();
        bool collidingWithBottom = m_ai.CollidingWithBottom();

        if (m_ball.IsSpawning())
        {
            m_ai.transform.position =
                Vector3.MoveTowards(
                    m_ai.transform.position,
                    new Vector3(50, 0, 0),
                    m_aiSpeed * Time.deltaTime);
            return;
        }

        if (!collidingWithTop && !collidingWithBottom
            || collidingWithTop && direction < 0
            || collidingWithBottom && direction > 0)
        {
            if (direction == 0)
            {
                m_ai.transform.position =
                    Vector3.MoveTowards(
                        m_ai.transform.position,
                        new Vector3(50, m_ball.transform.position.y, 0),
                        m_aiSpeed * Time.deltaTime);
                return;
            }

            m_ai.transform.Translate(new Vector3(0, direction * m_aiSpeed * Time.deltaTime, 0));
        }
    }
}
