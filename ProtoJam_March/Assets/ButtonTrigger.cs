using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    public TypeOfButton ButtonType;     //버튼 타입 지정
    public UnityEvent OnButtonPressed;  //버튼이 눌렸을 때 실행할 함수(잠긴문 열기 라든지...)
    public UnityEvent OnButtonRelease;  //버튼이 풀렸을 때 실행할 함수(홀드 버튼용)

    [SerializeField] private List<Transform> objectWhoPressedButton; //버튼을 누르고 있는 객체 리스트
    private bool isButtonPressed = false;   //(내부용)버튼 눌렀는가?

    
    
    //이하 Trigger Enter/Exit 은 버튼 누른 오브젝트 감지해서 리스트에 추가
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" || col.tag == "PickupBox")
        {
            objectWhoPressedButton.Add(col.transform);

            if (!isButtonPressed)
            {
                isButtonPressed = true;
                
                OnButtonPressed.Invoke();
                //버튼 눌렸을 때 버튼 애니메이션
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "PickupBox")
        {
            objectWhoPressedButton.Remove(other.transform);

            if (ButtonType == TypeOfButton.Hold && objectWhoPressedButton.Count <= 0)
            {
                isButtonPressed = false;
                //버튼 풀렸을 때 버튼 애니메이션
            }
        }
    }
    
    //버튼 타입 지정용 
    public enum TypeOfButton
    {
        Once = 1,
        Hold = 2
    }
    
    //Debug Method------------------------------------------------------------------------
    public void Debug_PrintString(string _str)
    {
        Debug.Log(_str);
    }
}
