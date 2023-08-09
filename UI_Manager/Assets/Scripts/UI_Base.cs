using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//모든 UI의 조상이 되는 Class
public abstract class UI_Base : MonoBehaviour
{
    //_object는 Key와 Value를 가지는 Dictionary이며, Key로는 Type, Value로는 
    //Key에 해당하는 object를 담은 배열을 가진다. Scene 상의 object를 로드하여 보관하는 목적
    protected Dictionary<Type, UnityEngine.Object[]> _object = new Dictionary<Type, UnityEngine.Object[]>();   


    //T 에는 Object, 혹은 Component가 들어갈 수 있다. Image, GameObject 등
    //T의 의미는, Generic Type으로 둔다는 명시적 의미다?
    //Generic Type은, Type을 미리 정하지 않는 대신, 사용할 시점에 Type을 정의하는 것
    //이 함수에 서로 다른 Type이 들어갈 수 있으니, 함수 사용 시에 Type을 정의한다
    protected void Bind<T>(Type type) where T : UnityEngine.Object {

        //Type들의 이름들을 가져와, names 배열에 저장
        string[] names = Enum.GetNames(type);

        //names.Length, 즉 Type의 종류의 수 만큼 UnityEngine.Object를 담을 배열 길이 설정
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        //Dictionary에 Add, Key는 T의 Type, Value는 objects(Object들을 담을 배열) 
        _object.Add(typeof(T), objects);



        // //T에 포함되는 Ojbect들을 배열 objects 에 넣어주기
        for (int i = 0; i < names.Length; i++) {

            //T가 component가 없는 빈 GameObject인 경우, 따로 함수 사용
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);

            //Generic Type을 사용한 FindChild<T>(..) 함수를 통해, names[i]와 같은 name을 가진 component들 받기 
            else {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            //component를 가져오지 못했을 경우
            if (objects[i] == null) {
                print($"{names[i]} Bind 오류");
            }


        }
    }
    
    //T 컴포넌트를 가지고 있으며 (혹은 오브젝트) 파라미터로 넘긴 int (예를들어 “Images.ItemIcon”을 int 로 형변환하면 enum 답게 그 정수가 리턴된다.)
    // idx에 해당하는 오브젝트를 T 타입으로 리턴함, 
    protected T Get<T>(int idx) where T : UnityEngine.Object {

        
        UnityEngine.Object[] objects = null;

        //T가 Key로 존재하면 true, out objects 를 통해 objects 배열에 Key의 Value 넣음
        if (_object.TryGetValue(typeof(T), out objects) == false) {

            return null;
        }


        return objects[idx] as T;
    }


    //각 Type 별로 return 해주는 함수??
    //다른 Script에서 Get 함수와 아래의 Type 별 Get 함수를 통해 object를 얻을 수 있다.
	protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
	protected Text GetText(int idx) { return Get<Text>(idx); }
	protected Button GetButton(int idx) { return Get<Button>(idx); }
	protected Image GetImage(int idx) { return Get<Image>(idx); }





}
