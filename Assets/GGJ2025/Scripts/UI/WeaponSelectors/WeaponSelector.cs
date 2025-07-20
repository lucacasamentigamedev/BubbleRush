using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine.UIElements;

public enum E_Resize
{
    GROWN,
    REDUCE,
    MINIMIZE,
    MAXIMIZE
}
public enum E_Move
{
    UP,
    DOWN,
    MIDDLE,
    HIDE_UP,
    HIDE_DOWN
}

public enum E_ICON_POSITION
{
    HIDE_UP = 0,
    UP = 1,
    MIDDLE = 2,
    DOWN = 3,
    HIDE_DOWN = 4
}


public class WeaponSelector : MonoBehaviour
{
    
    public E_ICON_POSITION Position { get; set; }

    private Coroutine resizeCoroutine;
    private Coroutine moveCoroutine;
    /*
    public void Resize(E_Resize _resize)
    {
        if (resizeCoroutine != null)
        {
            StopCoroutine(resizeCoroutine);
        }
        resizeCoroutine = StartCoroutine(ResizeWithDelay(_resize));
    }
    public void Move(E_Move _move)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveWithDelay(_move));
    }

    private IEnumerator ResizeWithDelay(E_Resize _resize)
    {
        Vector3 resizeFactor = Vector3.zero;
        Vector3 limitScale = Vector3.zero;
        switch(_resize)
        {
            case E_Resize.GROWN:
                    resizeFactor = new Vector3(0.006f, 0.006f, 0f);
                    limitScale = new Vector3(0.7f, 0.7f, 1f);
                    while (gameObject.transform.localScale.x <= limitScale.x)
                    {
                        gameObject.transform.localScale += resizeFactor;
                        yield return new WaitForEndOfFrame();
                    }
                break;
            case E_Resize.REDUCE:
                    resizeFactor = new Vector3(-0.006f, -0.006f, 0f);
                    limitScale = new Vector3(0.7f, 0.7f, 1f);
                    while (gameObject.transform.localScale.x >= limitScale.x)
                    {
                        gameObject.transform.localScale += resizeFactor;
                        yield return new WaitForEndOfFrame();
                    }
                break; 
            case E_Resize.MINIMIZE:
                    resizeFactor = new Vector3(-0.006f, -0.006f, 0f);
                    limitScale = new Vector3(0.1f, 0.1f, 1f);
                    while (gameObject.transform.localScale.x >= limitScale.x)
                    {
                        gameObject.transform.localScale += resizeFactor;
                        yield return new WaitForEndOfFrame();
                    }
                break; 
            case E_Resize.MAXIMIZE:
                    resizeFactor = new Vector3(0.006f, 0.006f, 0f);
                    limitScale = new Vector3(0.9f, 0.9f, 1f);
                    while (gameObject.transform.localScale.x <= limitScale.x)
                    {
                        gameObject.transform.localScale += resizeFactor;
                        yield return new WaitForEndOfFrame();
                    }
                break; 
            default: 
                break;

        }
    }
    private IEnumerator MoveWithDelay(E_Move _move)
    {
        Vector3 posYFactor = Vector3.zero;
        float limitY = 0.0f;
        Func<float, float, bool> _condition;
        switch (_move)
        {
            case E_Move.UP:
                    limitY = 1.1f;
                    posYFactor = new Vector3(0.0f, 0.01f, 0.0f);
                    _condition = (x, y) => x <= y;
                break;
            case E_Move.DOWN:
                    limitY = -1.1f;
                    posYFactor = new Vector3(0.0f, -0.01f, 0.0f);
                    _condition = (x, y) => x >= y;
                break;
            case E_Move.MIDDLE:
                    limitY = 0.0f;
                    posYFactor = gameObject.transform.position.y < limitY ? new Vector3(0.0f, 0.01f, 0.0f) : new Vector3(0.0f, -0.01f, 0.0f);
                    _condition = (x, y) => Math.Abs(x - y)>=0.000001f;
                break;


            case E_Move.HIDE_UP:
                    limitY = 1.6f;
                    posYFactor = new Vector3(0.0f, 0.01f, 0.0f);
                    _condition = (x, y) => x <= y;
                break;
            case E_Move.HIDE_DOWN:
                    limitY = -1.6f;
                    posYFactor = new Vector3(0.0f, -0.01f, 0.0f);
                    _condition = (x, y) => x >= y;
                break;


            default:
                 _condition = (x, y) => true; 
                break;
        }

        while (_condition(gameObject.transform.position.y, limitY))
        {
            gameObject.transform.position += posYFactor;
            yield return new WaitForEndOfFrame();
        }
    }
    */

    private IEnumerator MoveUpWithDelay(E_ICON_POSITION _movePosition, bool up)
    {
        Vector3 posYFactor = Vector3.zero;
        float limitY = 0.0f;
        Vector3 resizeFactor = Vector3.zero;
        float limitScale = 0;
        Func<float, float, bool> _posCondition;
        Func<float, float, bool> _scaleCondition;
        switch (_movePosition)
        {
            case E_ICON_POSITION.HIDE_UP:
                limitY = 1.6f;
                limitScale = 0.1f;
                posYFactor = new Vector3(0.0f, 0.01f, 0.0f);
                _posCondition = (x, y) => x <= y;
                if (up)
                {
                    resizeFactor = new Vector3(-0.006f, -0.006f, 0f);
                    _scaleCondition = (x, y) => x >= y;                    
                }
                else
                {
                    _scaleCondition = (x, y) => true;
                }
                break;
            case E_ICON_POSITION.UP:
                limitY = 1.1f;
                limitScale = 0.7f;
                
                if (up)
                {
                    posYFactor = new Vector3(0.0f, 0.01f, 0.0f);
                    _posCondition = (x, y) => x <= y;

                    resizeFactor = new Vector3(-0.006f, -0.006f, 0f);
                    _scaleCondition = (x, y) => x >= y;
                }
                else
                {
                    posYFactor = new Vector3(0.0f, -0.01f, 0.0f);
                    _posCondition = (x, y) => x >= y;

                    resizeFactor = new Vector3(0.006f, 0.006f, 0f);
                    _scaleCondition = (x, y) => x <= y;
                }
                break;
            case E_ICON_POSITION.MIDDLE:
                limitY = 0.0f;
                limitScale = 0.9f;
                posYFactor = gameObject.transform.position.y < limitY ? new Vector3(0.0f, 0.01f, 0.0f) : new Vector3(0.0f, -0.01f, 0.0f);
                _posCondition = (x, y) => Math.Abs(x - y) >= 0.000001f;
                if (up)
                {
                    resizeFactor = new Vector3(0.006f, 0.006f, 0f);
                    _scaleCondition = (x, y) => x <= y;
                }
                else
                {
                    resizeFactor = new Vector3(0.006f, 0.006f, 0f);
                    _scaleCondition = (x, y) => x <= y;
                }
                break;
            case E_ICON_POSITION.DOWN:
                limitY = -1.1f;
                limitScale = 0.7f;                
                if (up)
                {
                    posYFactor = new Vector3(0.0f, 0.01f, 0.0f);
                    _posCondition = (x, y) => x <= y;

                    resizeFactor = new Vector3(0.006f, 0.006f, 0f);
                    _scaleCondition = (x, y) => x <= y;
                }
                else
                {
                    posYFactor = new Vector3(0.0f, -0.01f, 0.0f);
                    _posCondition = (x, y) => x >= y;

                    resizeFactor = new Vector3(-0.006f, -0.006f, 0f);
                    _scaleCondition = (x, y) => x >= y;
                }
                break;
            case E_ICON_POSITION.HIDE_DOWN:
                limitY = -1.6f;
                limitScale = 0.1f;
                posYFactor = new Vector3(0.0f, -0.01f, 0.0f);
                _posCondition = (x, y) => x >= y;
                if (up)
                {
                    _scaleCondition = (x, y) => true;
                }
                else
                {
                    resizeFactor = new Vector3(-0.006f, -0.006f, 0f);
                    _scaleCondition = (x, y) => x >= y;
                }
                break;


            default:
                _posCondition = (x, y) => true;
                _scaleCondition = (x, y) => true;
                break;
        }

        while (_posCondition(gameObject.transform.position.y, limitY) || _scaleCondition(gameObject.transform.localScale.x, limitScale))
        {
            if(_posCondition(gameObject.transform.position.y, limitY))
                gameObject.transform.position += posYFactor;
            if(_scaleCondition(gameObject.transform.localScale.x, limitScale))
                gameObject.transform.localScale += resizeFactor;
            yield return new WaitForEndOfFrame();
        }
    }

    public void MoveUp()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        switch (Position)
        {
            case E_ICON_POSITION.HIDE_UP:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, -1.6f, gameObject.transform.position.z);
                break;
            case E_ICON_POSITION.UP:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.HIDE_UP,true));
                break;
            case E_ICON_POSITION.MIDDLE:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.UP, true));
                break;
            case E_ICON_POSITION.DOWN:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.MIDDLE, true));
                break;
            case E_ICON_POSITION.HIDE_DOWN:                
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.DOWN, true));
                break;

        }
    }

    public void MoveDown()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        switch (Position)
        {
            case E_ICON_POSITION.HIDE_UP:                
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.UP, false));
                break;
            case E_ICON_POSITION.UP:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.MIDDLE, false));
                break;
            case E_ICON_POSITION.MIDDLE:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.DOWN, false));
                break;
            case E_ICON_POSITION.DOWN:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.HIDE_DOWN, false));
                break;
            case E_ICON_POSITION.HIDE_DOWN:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1.6f, gameObject.transform.position.z);                
                break;
        }
    }
}
