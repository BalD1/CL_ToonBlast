using UnityEngine;

public static class GameObjectExtensions
{
    #region Vector2

    /// <summary>
    /// Creates the given <paramref name="gameObject"/> at <paramref name="position"/>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The created <paramref name="gameObject"/></returns>
    public static GameObject Create(this GameObject gameObject, Vector2 position)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, position, Quaternion.identity);
        return gO;
    }
    /// <summary>
    /// Creates the given <paramref name="gameObject"/> at <paramref name="position"/> with <paramref name="quaternion"/> rotation
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The created <paramref name="gameObject"/></returns>
    public static GameObject Create(this GameObject gameObject, Vector2 position, Quaternion quaternion)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, position, quaternion);
        return gO;
    }

    /// <summary>
    /// Creates the given <paramref name="gameObject"/> at <paramref name="position"/>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The <typeparamref name="T"/> component of <paramref name="gameObject"/></returns>
    public static T Create<T>(this GameObject gameObject, Vector2 position)
    {
        if (gameObject == null) return default;
        GameObject gO = GameObject.Instantiate(gameObject, position, Quaternion.identity);
        return gO.GetComponent<T>();
    }
    /// <summary>
    /// Creates the given <paramref name="gameObject"/> at <paramref name="position"/> with <paramref name="quaternion"/> rotation
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The <typeparamref name="T"/> component of <paramref name="gameObject"/></returns>
    public static T Create<T>(this GameObject gameObject, Vector2 position, Quaternion quaternion)
    {
        if (gameObject == null) return default;
        GameObject gO = GameObject.Instantiate(gameObject, position, quaternion);
        return gO.GetComponent<T>();
    }

    #endregion

    #region Vector3

    /// <summary>
    /// Creates the given <paramref name="gameObject"/> at <paramref name="position"/>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The created <paramref name="gameObject"/></returns>
    public static GameObject Create(this GameObject gameObject, Vector3 position)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, position, Quaternion.identity);
        return gO;
    }
    /// <summary>
    /// Creates the given <paramref name="gameObject"/> at <paramref name="position"/> with <paramref name="quaternion"/> rotation
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The created <paramref name="gameObject"/></returns>
    public static GameObject Create(this GameObject gameObject, Vector3 position, Quaternion quaternion)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, position, quaternion);
        return gO;
    }

    /// <summary>
    /// Creates the given <paramref name="gameObject"/> at <paramref name="position"/>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The <typeparamref name="T"/> component of <paramref name="gameObject"/></returns>
    public static T Create<T>(this GameObject gameObject, Vector3 position)
    {
        if (gameObject == null) return default;
        GameObject gO = GameObject.Instantiate(gameObject, position, Quaternion.identity);
        return gO.GetComponent<T>();
    }
    /// <summary>
    /// Creates the given <paramref name="gameObject"/> at <paramref name="position"/> with <paramref name="quaternion"/> rotation
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The <typeparamref name="T"/> component of <paramref name="gameObject"/></returns>
    public static T Create<T>(this GameObject gameObject, Vector3 position, Quaternion quaternion)
    {
        if (gameObject == null) return default;
        GameObject gO = GameObject.Instantiate(gameObject, position, quaternion);
        return gO.GetComponent<T>();
    }

    #endregion

    #region Transform

    /// <summary>
    /// Creates the given <paramref name="gameObject"/> with <paramref name="parent"/> as parent
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The created <paramref name="gameObject"/></returns>
    public static GameObject Create(this GameObject gameObject, Transform parent)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, parent);
        return gO;
    }
    /// <summary>
    /// Creates the given <paramref name="gameObject"/> with <paramref name="parent"/> as parent
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The created <paramref name="gameObject"/></returns>
    public static GameObject Create(this GameObject gameObject, RectTransform parent)
    {
        if (gameObject == null) return null;
        GameObject gO = GameObject.Instantiate(gameObject, parent);
        return gO;
    }

    /// <summary>
    /// Creates the given <paramref name="gameObject"/> with <paramref name="parent"/> as parent
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The <typeparamref name="T"/> component of <paramref name="gameObject"/></returns>
    public static T Create<T>(this GameObject gameObject, Transform parent)
    {
        if (gameObject == null) return default;
        GameObject gO = GameObject.Instantiate(gameObject, parent);
        return gO.GetComponent<T>();
    }
    /// <summary>
    /// Creates the given <paramref name="gameObject"/> with <paramref name="parent"/> as parent
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position"></param>
    /// <returns>The <typeparamref name="T"/> component of <paramref name="gameObject"/></returns>
    public static T Create<T>(this GameObject gameObject, RectTransform parent)
    {
        if (gameObject == null) return default;
        GameObject gO = GameObject.Instantiate(gameObject, parent);
        return gO.GetComponent<T>();
    } 

    #endregion
}
