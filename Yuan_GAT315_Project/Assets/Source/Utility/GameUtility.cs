using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUtility : MonoBehaviour
{

    public static Vector3[] SubdivideCircleEdgePoints(Vector3 center, float radius, int numPoints, float startingAngleOffset)
    {
        Vector3[] points = new Vector3[numPoints]; // Points to return 

        for (int i = 0; i < numPoints; ++i)
        {
            float angle = i * Mathf.PI * 2f / numPoints + startingAngleOffset * Mathf.Deg2Rad;
            points[i] = new Vector3(center.x + Mathf.Cos(angle) * radius, center.y + Mathf.Sin(angle) * radius, center.z);
        }

        return points;
    }

    public static void DestroyChidren(Transform parent, bool immediate = false)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent)
            children.Add(child.gameObject);

        foreach (GameObject child in children)
        {
            SafeDestroy(child, immediate);
        }
    }

    public static T SafeDestroy<T>(T gameObject, bool immediate = false) where T : Object
    {
        if (gameObject == null)
            return null;

        if (Application.isEditor || immediate)
            DestroyImmediate(gameObject);
        else
            Destroy(gameObject);

        return null;
    }
    public static Transform[] GetChildren(Transform parent)
    {
        Transform[] positions = new Transform[parent.childCount];
        for (int i = 0; i < parent.childCount; ++i)
            positions[i] = parent.transform.GetChild(i);

        return positions;
    }

    public static float HyperbolicArmor(float armor, float halfPoint)
    {
        return 1 - (armor / (halfPoint + armor));  
    }

    public static int ToInt(bool val)
    {
        return val ? 1 : 0;
    }

    public static GameObject ShootProjectile(GameObject projectile, float speed, Transform origin, Vector3 dir)
    {
        GameObject clone = Instantiate(projectile, origin.position, origin.rotation);

        dir.Normalize();

        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = dir * speed;

        return clone;
    }

    public static GameObject ShootProjectileMouse(GameObject projectile, float speed, Transform origin)
    {
        return ShootProjectile(projectile, speed, origin, Input.mousePosition - origin.position);
    }

    public static IEnumerator WaitForAnimation(Animation animation)
    {
        do
            yield return null;
        while (animation.isPlaying);
    }

    public static void SetVisible<T>(ref T[] obj, bool visible)
    {
        if (obj == null)
            return;

        foreach (T item in obj)
        {
            object o = item;

            if (typeof(T) == typeof(Text))
            {
                ((Text)o).enabled = visible;
                continue;
            }
            if (typeof(T) == typeof(SpriteRenderer))
            {
                ((SpriteRenderer)o).enabled = visible;
                continue;
            }
            if (typeof(T) == typeof(Image))
            {
                ((Image)o).enabled = visible;
            }
        }
    }
}
