using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    public Animator anim;
    private bool _ground;
    private bool _inRoom;

	void Start()
    {
        anim.enabled = false;
        _ground = false;
        _inRoom = false;
    }
	
	void Update()
    {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 10)
        {
            anim.enabled = true;
            StartCoroutine(RefreshImage());
        }
    }

    IEnumerator RefreshImage()
    {
        _ground = true;
        anim.SetBool("Down", true);
        yield return new WaitForSeconds(0.5f);
        if (_inRoom)
        {
            NotificationCenter.DefaultCenter.PostNotification("AddPraiseNotify", true);
        }
        else
        {
            anim.SetBool("Walk", true);
            NotificationCenter.DefaultCenter.PostNotification("AddPraiseNotify", false);
            var gameObjs = GameObject.FindGameObjectsWithTag("Home");
            var homeObj = gameObjs[0];
            for (int i = 1; i < gameObjs.Length; i++)
            {
                var distance1 = Vector2.Distance(transform.localPosition, homeObj.transform.localPosition);
                var distance2 = Vector2.Distance(transform.localPosition, gameObjs[i].transform.localPosition);
                if (distance2 < distance1)
                {
                    homeObj = gameObjs[i];
                }
            }
            transform.DOMove(new Vector2(homeObj.transform.localPosition.x, transform.localPosition.y), 2);
            if (homeObj.transform.localPosition.x < transform.localPosition.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            yield return new WaitForSeconds(2f);
            anim.SetBool("Down", false);
            anim.SetBool("Stop", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            _inRoom = true;
        }
    }
}
