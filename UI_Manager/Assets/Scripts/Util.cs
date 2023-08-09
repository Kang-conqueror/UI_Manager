using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//UI에서 공용적으로 자주 사용할 함수를 정의한 Script 
public class Util
{
    
    //여기서도 Generic 표현법 T를 이용해, 다양한 형태의 Type을 받고 반환할 수 있도록 함
    //whree T : UnityEngine.object 를 통해 T의 제약조건, 즉 가능한 Type을 설정해놓기
    //go 의 Childs 중, T component를 보유하며 name과 이름이 일치한 object를 return하는 함수
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        //object가 null이면, return null
        if (go == null){
            return null;
        }

        //recursive가 false면, 직속 자손, 바로 아래의 child 에서만 탐색
        if (recursive == false) {

            //go의 childCount, 자식의 수 만큼 반복문 돌려 탐색
            for (int i = 0; i < go.transform.childCount; i++){

                //go의 child 중, i번째 idx의 transform 값 가져오기
                Transform transform = go.transform.GetChild(i);

                //child의 name이 우리가 목표로하는 name과 같은 경우
                if (string.IsNullOrEmpty(name) || transform.name == name) {
                    
                    //목표로 한 child의 T Type Component를 return 해주기
                    T component = transform.GetComponent<T>();
                    if (component != null) {
                        return component;
                    }
                }
            }
        }

        //recursive가 true면, child, child의 child... 모든 자식을 범위로 탐색
        else {

            //모든 child 에서 T Type Component를 가져와 탐색하기
            foreach (T component in go.GetComponentsInChildren<T>()){

                //목표로 하는 name과 같다면, 그 component return 해주기
                if (string.IsNullOrEmpty(name) || component.name == name) {

                    return component;
                }
            }
        }

        return null;
    }


    //component를 가지고 있지 않은 GameObject Type의 object를 검사할 함수
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false) {

        //바로 위에서 정의한 T FindChild<T>(..) 함수를 사용
        Transform transform = FindChild<Transform>(go, name, recursive);
        
        if (transform == null) {
            return null;
        }

        return transform.gameObject;
    }

    //object에서 특정 T Component를 가져오거나, 없으면 Add 후 가져오는 함수.
    //Get과 Add를 붙여서 하나의 함수로 사용
    public static T GetorAddComponent<T>(GameObject go) where T : UnityEngine.Component {

        //getcomponent로 가져오기
        T component = go.GetComponent<T>();

        //가져오지 못했을 경우, 즉 component가 없을 경우
        if (component == null) {

            //Addcomponent로 더해주기
            component = go.AddComponent<T>();
        }

        return component;
    }





}
