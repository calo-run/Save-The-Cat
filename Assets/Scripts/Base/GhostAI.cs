using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public SkeletonAnimation spinAnim;
    public AIPath aiPath;
    public CatController target;
    public float speed = 200f;
    public float nextWaypointDistance = 3l;
    private Path path;
    private int currentWaypoint;
    private bool reachedEndOfPath;
    private Seeker seeker;
    private Rigidbody2D rb;
    private List<CatController> cats = new List<CatController>();
    private bool auto = true;
    void Start()
    {
        spinAnim.AnimationState.SetAnimation(0, "animation", true);
        speed += Random.Range(0, 50);
        foreach (CatController _dog in FindObjectsOfType<CatController>())
        {
            cats.Add(_dog);
        }

        target = cats[Random.Range(0, cats.Count - 1)];
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        auto = true;
        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath(){
        while(!GameController.Instance.b_EndGame)
        {
            yield return new WaitForSeconds(0.2f);
            if (auto)
            {
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
            }
        }
        
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        while(GameController.Instance.b_EndGame)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if(path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        rb.velocity = direction * speed * Time.deltaTime;
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        if (target.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    
    private float m_Thrust = 50f;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("dog"))
        {
            other.gameObject.GetComponent<IHit>().OnHit();
        }
        if (other.gameObject.tag == "line")
        {
            auto = false;
            Vector2 direction = (target.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * m_Thrust);
            rb.AddForce(-1*direction*100);
            foreach (CatController cat in cats)
            {
                cat.RunAnimScary();
            }

            StartCoroutine(AutoOn());
        }
    }

    IEnumerator AutoOn()
    {
        yield return new WaitForSeconds(0.3f);
        auto = true;
    }
}
