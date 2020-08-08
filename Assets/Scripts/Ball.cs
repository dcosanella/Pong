using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private enum BallDirections : int
    {
        Middle = 0,
        Up,
        Down
    }

    private readonly Vector3[] m_rightDirections =
    {
        Vector3.right,
        new Vector3(1, 0.5f, 0),
        new Vector3(1, -0.5f, 0),
    };

    private readonly Vector3[] m_leftDirections =
    {
        Vector3.left,
        new Vector3(-1, 0.5f, 0),
        new Vector3(-1, -0.5f, 0)
    };

    private Vector3 m_ballDirection;
    private BoxCollider2D m_collider;
    private Paddle m_currentPaddle;
    private UIManager m_uiManager;

    private int m_speed = 40;
    private int m_serveSpeed = 25;

    private bool m_isSpawning;
    private bool m_serving;

    // Start is called before the first frame update
    void Start()
    {
        m_uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (m_uiManager == null)
        {
            Debug.LogException(new System.NullReferenceException());
        }

        m_collider = gameObject.GetComponent<BoxCollider2D>();
        if (m_collider == null)
        {
            Debug.LogException(new System.NullReferenceException());
        }

        m_currentPaddle = GameObject.Find("AI").GetComponent<Paddle>();
        if (m_currentPaddle == null)
        {
            Debug.LogException(new System.NullReferenceException());
        }

       RespawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_serving)
        {
            transform.Translate(m_ballDirection * m_serveSpeed * Time.deltaTime);
            return;
        }

        transform.Translate(m_ballDirection * m_speed * Time.deltaTime);
    }

    public float GetBallDirection()
    {
        return m_ballDirection.y;
    }

    public bool IsSpawning()
    {
        return m_isSpawning;
    }

    private void RespawnBall()
    {
        int count = System.Enum.GetValues(typeof(BallDirections)).Length;
        int direction = Random.Range(1, System.Enum.GetValues(typeof(BallDirections)).Length);

        transform.position = new Vector3(0, Random.Range(-10, 10), 0);
        if (m_currentPaddle.name == "Player")
        {
            m_ballDirection = m_rightDirections[direction];
        }
        else
        {
            m_ballDirection = m_leftDirections[direction];
        }

        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        m_isSpawning = false;
        m_serving = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Paddle")
        {
            if (m_serving)
            {
                m_serving = false;
            }

            ChangeBallDirection(other);
            return;
        }

        if (other.tag == "Wall_Top" || other.tag == "Wall_Bottom")
        {
            if (m_currentPaddle.name == "Player")
            {
                DeflectBall(m_rightDirections);
                return;
            }

            DeflectBall(m_leftDirections);
        }

        if (other.tag == "Goal")
        {
            //m_ballDirection = Vector3.zero;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            m_currentPaddle.UpdateScore();
            m_uiManager.UpdateScore(m_currentPaddle.name, m_currentPaddle.GetScore());
            StartCoroutine(RespawnBallRoutine());
        }
    }

    private void ChangeBallDirection(Collider2D other)
    {
        Paddle paddle = GameObject.Find(other.gameObject.name).GetComponent<Paddle>();
        if (paddle == null)
        {
            Debug.LogException(new System.NullReferenceException());
        }

        paddle.GetBallCollision(other);

        Paddle.PaddleCollider status = paddle.GetColliderStatus();
        switch (status)
        {
            case Paddle.PaddleCollider.Top:
                FlipBallDirectionUp(paddle.gameObject.name);
                break;
            case Paddle.PaddleCollider.Middle:
                FlipBallDirection(paddle.gameObject.name);
                break;
            case Paddle.PaddleCollider.Bottom:
                FlipBallDirectionDown(paddle.gameObject.name);
                break;
            default:
                break;
        }

        m_currentPaddle = paddle;
    }

    private void FlipBallDirectionUp(string paddleName)
    {
        if (paddleName == "Player")
        {
            m_ballDirection = m_rightDirections[(int)BallDirections.Up];
            return;
        }

        m_ballDirection = m_leftDirections[(int)BallDirections.Up];
    }

    private void FlipBallDirection(string paddleName)
    {
        if (paddleName == "Player")
        {
            m_ballDirection = m_rightDirections[(int)BallDirections.Middle];
            return;
        }

        m_ballDirection = m_leftDirections[(int)BallDirections.Middle];
    }

    private void FlipBallDirectionDown(string paddleName)
    {
        if (paddleName == "Player")
        {
            m_ballDirection = m_rightDirections[(int)BallDirections.Down];
            return;
        }

        m_ballDirection = m_leftDirections[(int)BallDirections.Down];
    }

    private void DeflectBall(Vector3[] directions)
    {
        var direction = int.MinValue;
        for(var i = 0; i < directions.Length; i++)
        {
            if (m_ballDirection == directions[i])
            {
                direction = i;
                break;
            }
        }

        if (direction == int.MinValue)
        {
            throw new System.IndexOutOfRangeException();
        }

        if (direction % 2 == 0)
        {
            m_ballDirection = directions[direction - 1];
            return;
        }

        m_ballDirection = directions[direction + 1];
    }

    IEnumerator RespawnBallRoutine()
    {
        m_isSpawning = true;
        yield return new WaitForSeconds(2);
        RespawnBall();
    }

    // TODO: Add OnTriggerEnter2D for all walls
    // Left and right walls: destroy ball, update score, spawn new ball
    // Top and bottom walls: deflect ball at 45 degree angle
}
