using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public static DrawManager Instance;
    private Camera _Cam;
    [SerializeField] Line _LinePrefabs;
    public const float Resolusion = 0.2f;
    private Line _CurrentLine;
    private bool canDraw;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _Cam = Camera.main;
    }

    void Update()
    {
        if (canDraw && GameController.Instance.GetLevelDesign() != null)
        {
            Vector2 mousePos = _Cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                GameController.Instance.GetLevelDesign().UnActiveHint();
                _CurrentLine = Instantiate(_LinePrefabs, mousePos, Quaternion.identity);
                _CurrentLine.transform.SetParent(GameController.Instance.GetLevelDesign().transform);
                
            }
            if (Input.GetMouseButton(0) && _CurrentLine != null && canDraw)
            {
                _CurrentLine.SetPos(mousePos);
            }
            if (Input.GetMouseButtonUp(0) && _CurrentLine != null && _CurrentLine._Render.positionCount > 1)
            {
                EndDraw();
            }
        }
    }

    public void EndDraw()
    {
        canDraw = false;
        _CurrentLine.EndDraw();
        DropLine();
        GameController.Instance.PlayGameAfterDraw();
    }

    public void DropLine()
    {
        if (_CurrentLine.gameObject)
        {
            _CurrentLine.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            _CurrentLine.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            _CurrentLine.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.2f;
        }
    }
    public void ResetLine()
    {
        _CurrentLine = null;
    }

    public void OnCanDraw()
    {
        canDraw = true;
    }
}
