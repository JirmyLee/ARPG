using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//打瞌睡状态

//添加右键菜单，在Asset文件夹下，鼠标右键菜单栏中添加一个按钮项，菜单名为 menuName，并执行生成名为 fileName 的脚本，order 为按钮显示顺序
[CreateAssetMenu(fileName = "AISleep",menuName = "StateMachine/State/AISleep")]
public class AISleep : StateActionSO
{
    public override void OnUpdate()
    {
        Debug.Log("AI State: Sleep");
    }
}
