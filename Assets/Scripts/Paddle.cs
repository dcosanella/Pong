using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public enum PaddleCollider
    {
        Middle,
        Top,
        Bottom
    }

    private BoxCollider2D m_topCollider;
    private BoxCollider2D m_middleCollider;
    private BoxCollider2D m_bottomCollider;

    private PaddleCollider m_colliderStatus;

    private bool m_collidingWithTop;
    private bool m_collidingWithBottom;

    private int m_score;

    // Start is called before the first frame update
    void Start()
    {
        var colliders = gameObject.GetComponents<BoxCollider2D>();
        if (colliders == null)
        {
            Debug.LogException(new System.NullReferenceException());
        }

        AssignColliders(colliders);

        m_score = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Contains("Wall_"))
        {
            CheckWallCollisions(other);
            return;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Contains("Wall_"))
        {
            ResetWallCollision(other);
        }
    }

    void AssignColliders(BoxCollider2D[] colliders)
    {
        m_topCollider = colliders[0];
        m_middleCollider = colliders[1];
        m_bottomCollider = colliders[2];
    }

    void CheckWallCollisions(Collider2D other)
    {
        if (other.tag == "Wall_Top")
        {
            m_collidingWithTop = true;
        }

        if (other.tag == "Wall_Bottom")
        {
            m_collidingWithBottom = true;
        }
    }

    void ResetWallCollision(Collider2D other)
    {
        if (other.tag == "Wall_Top")
        {
            m_collidingWithTop = false;
        }

        if (other.tag == "Wall_Bottom")
        {
            m_collidingWithBottom = false;
        }
    }

    public void GetBallCollision(Collider2D other)
    {
        if (other == m_topCollider)
        {
            m_colliderStatus = PaddleCollider.Top;
        }
        else if (other == m_middleCollider)
        {
            m_colliderStatus = PaddleCollider.Middle;
        }
        else if (other == m_bottomCollider)
        {
            m_colliderStatus = PaddleCollider.Bottom;
        }
    }

    public PaddleCollider GetColliderStatus()
    {
        return m_colliderStatus;
    }

    public bool CollidingWithTop()
    {
        return m_collidingWithTop;
    }

    public bool CollidingWithBottom()
    {
        return m_collidingWithBottom;
    }

    public void UpdateScore()
    {
        m_score++;
    }

    public int GetScore()
    {
        return m_score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
