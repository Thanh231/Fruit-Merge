using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_ins;
    public static T Ins
    {
        get
        {
            if (m_ins == null)
            {
                GameObject singleton = new GameObject(typeof(T).Name);
                m_ins = singleton.AddComponent<T>();
            }
            return m_ins;
        }
    }

    protected virtual void Awake()
    {
        MakeSingleton(true);
    }

    public void MakeSingleton(bool isPersistent)
    {
        if (m_ins == null)
        {
            m_ins = this as T;

            if (isPersistent)
            {
                Transform root = transform.root;
                DontDestroyOnLoad(root.gameObject);
            }
        }
        else
        {
            if (m_ins != this)
            {
                Destroy(gameObject);
            }
        }
    }
}