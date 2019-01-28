using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite[] ending;
    private bool _isAllowedMove;
    private bool _ground;
    private float _angle;
    private bool _isAlive;
    private int _levelPraise;

    void Start()
    {
        _isAllowedMove = true;
        _ground = false;
        _isAlive = true;
        NotificationCenter.DefaultCenter.AddObserver(this, "SetAllowPlayerMove", AllowMove);
        NotificationCenter.DefaultCenter.AddObserver(this, "AddPraiseNotify", AddPraise);

        _levelPraise = GameController.GetLevelPraise();
        NotificationCenter.DefaultCenter.PostNotification("GameWndSetText", _levelPraise);
        NotificationCenter.DefaultCenter.PostNotification("GameWndSetSlider", 0f);
    }

    void OnDestroy()
    {
        NotificationCenter.DefaultCenter.RemoveObserver(this, "SetAllowPlayerMove");
        NotificationCenter.DefaultCenter.RemoveObserver(this, "AddPraiseNotify");
    }

    void FixedUpdate()
    {
        if (!_isAllowedMove)
            return;

        if (!_isAlive)
            return;

        if (_ground == true)
        {
            float mJumpSpeed = 60;
            var newVector = GetNewVector();
            GetComponent<Rigidbody2D>().velocity += newVector * mJumpSpeed;
            _angle = 0;
            NotificationCenter.DefaultCenter.PostNotification("GameWndSetSlider", _angle);
            _ground = false;
        }
    }

    void Update()
    {
        CheckInteraction();
        AddAngle();
        CheckIfRestart();
        ReleasePassenger();
        BreakRules();
    }

    private void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (WindowManager.GetWnd(UIMenu.ReturnCityWnd) == null)
            {
                WindowManager.Open(UIMenu.ReturnCityWnd);
                _isAllowedMove = false;
            }
            else
            {
                WindowManager.Close(UIMenu.ReturnCityWnd);
                _isAllowedMove = true;
            }
        }
    }

    private void AddAngle()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _angle += 0.4f;
            if (_angle > 45)
                _angle = 45;

            NotificationCenter.DefaultCenter.PostNotification("GameWndSetSlider", _angle);
        }
    }

    private void CheckIfRestart()
    {
        if (!_isAlive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                WindowManager.Close(UIMenu.ReturnCityWnd);
                Application.LoadLevelAsync(Application.loadedLevelName);
            }
        }
    }

    private void ReleasePassenger()
    {
        if (_isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Object obj = Resources.Load("Passengers/Passenger");
                var passenger = (GameObject.Instantiate(obj) as GameObject).transform;
                passenger.localPosition = transform.localPosition;
                var num = Random.Range(0, 4);
                passenger.GetComponent<SpriteRenderer>().sprite = sprites[num];
            }
        }
    }

    private void BreakRules()
    {
        if (_isAlive)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(PlayerMove());
            }
        }
    }

    IEnumerator PlayerMove()
    {
        var name = Application.loadedLevelName;
        if (name == "Level")
        {
            transform.DOMove(new Vector2(255f, -15f), 8f);
        }
        else if (name == "Level2")
        {
            transform.DOMove(new Vector2(441f, -15f), 8f);
        }
        else if (name == "Level3")
        {
            transform.DOMove(new Vector2(291f, -15f), 8f);
        }
        yield return new WaitForSeconds(8f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void Pass()
    {
        GameController.SetLevelPraise(_levelPraise);
        var name = Application.loadedLevelName;
        if (name == "Level")
        {
            WindowManager.Close(UIMenu.ReturnCityWnd);
            Application.LoadLevelAsync("Level2");
            Level.recordId = 0;
        }
        else if (name == "Level2")
        {
            WindowManager.Close(UIMenu.ReturnCityWnd);
            Application.LoadLevelAsync("Level3");
            Level.recordId = 0;
        }
        else if (name == "Level3")
        {
            WindowManager.Close(UIMenu.ReturnCityWnd);
            WindowManager.Close(UIMenu.GameWnd);
            WindowManager.Open(UIMenu.EndingWnd);
            Application.LoadLevelAsync("End");
        }
    }

    private Vector2 GetNewVector()
    {
        var x = Vector2.up.x;
        var y = Vector2.up.y;
        var sin = Mathf.Sin(Mathf.PI * _angle / 180);
        var cos = Mathf.Cos(Mathf.PI * _angle / 180);
        var newX = x * cos + y * sin;
        var newY = x * -sin + y * cos;
        var newVector = new Vector2(newX, newY);
        return newVector;
    }

    private void AllowMove(object[] parms)
    {
        var isAllowed = (bool)parms[0];
        _isAllowedMove = isAllowed;
    }

    private void AddPraise(object[] parms)
    {
        var add = (bool)parms[0];
        if (add)
        {
            _levelPraise++;
        }
        else
        {
            _levelPraise--;
            if (_levelPraise < 0)
                _levelPraise = 0;
        }
        NotificationCenter.DefaultCenter.PostNotification("GameWndSetText", _levelPraise);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 10 || col.gameObject.layer == 11)
        {
            transform.localEulerAngles = new Vector3(0, 0, 180f);
            _isAlive = false;
        }
        else if (col.gameObject.layer == 13)
        {
            _ground = true;
            GetComponent<Rigidbody2D>().velocity -= new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15)
        {
            Pass();
        }
    }
}
