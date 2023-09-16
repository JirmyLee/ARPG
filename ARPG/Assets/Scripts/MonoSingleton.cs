using UnityEngine;

//正常单例会自动实例化，但Mono单例需要绑定在对象上才能实例化
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }

    }

    private void Awake()
    {
        if (global)
        {
            //判断Mono单例是否创建过了，如果已经创建过就销毁这次创建的单例，反正每次进入主城就重复创建单例对象
            //这也是MonoBehaviour一般不能做成单例的原因:MonoBehaviour可以绑在对象上，每次进入场景创建对象就会再次创建MonoBehaviour 
            if ((instance != null) && (instance != this.gameObject.GetComponent<T>()))
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            instance = this.gameObject.GetComponent<T>();
        }
        this.OnAwake();
    }

    void Start()
    {
        //注：继承MonoSingleton的单例中不能直接使用Start，要重写OnStart方法(MonoSingleton的start会调用OnStart),否则子类会覆盖父类的，父类的start无法执行
        this.OnStart();
    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {

    }
}
