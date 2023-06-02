using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Одиночка
    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;
    void Awake()
    {
        S = this; // Определить одиночку
// Получить ссылку на LineRenderer
    line = GetComponent<LineRenderer>();
// Выключить LineRenderer пока он не понадобится
    line.enabled = false;
// Инициализировать список точек
    points = new List<Vector3>();
    }
    // Этот метод будет вызываться из класса Projectile
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;
        if ( points.Count > 0 && (pt - lastPoint).magnitude < minDist ) {
// Если точка недостаточно далека от предыдущей, просто выйти
            return;
        }
        if ( points.Count == 0 ) { // Если это точка запуска...
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // Для определения
// ...добавить дополнительный фрагмент линии,
// чтобы помочь лучше прицелиться в будущем
            points.Add(pt + launchPosDiff);
points.Add(pt);
line.positionCount = 2;
// Установить первые две точки
line.SetPosition(0, points[0]);
line.SetPosition(1, points[1]);
// Включить LineRenderer
line.enabled = true;
        }
        else
        {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if (poi == null)
        {
            if(FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        AddPoint();
        if (FollowCam.POI == null)
        {
            poi = null;
        }
    }

   
}
