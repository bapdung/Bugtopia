using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter2 : MonoBehaviour
{
    void Start()
    {
        // 이미 초기화가 완료된 'transform'이란 데이터 잇음
        Vector3 vec = new Vector3(0, 0, 0); // 벡터 만들기
        transform.Translate(vec); // 벡터 값 + 현재 위치 
    }

    void Update()
    {
        
    }
}
