// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class InsectController : MonoBehaviour
// {
//     public GameObject foodObject; // Food 오브젝트
//     private Animator insectAnimator; // Insect의 Animator
//     private bool isInsectMoving = false; // Insect가 Food로 이동 중인지 확인
//     private float rotationSpeed = 2.0f; // 회전 속도

//     void Start()
//     {
//         insectAnimator = GetComponent<Animator>();
//         if (insectAnimator != null)
//         {
//             insectAnimator.SetBool("idle", true); // 초기 상태를 idle로 설정
//         }
//     }

//     void Update()
//     {
//         // Insect가 Food를 향해 이동
//         if (isInsectMoving && foodObject != null)
//         {
//             MoveInsectTowardsFood();
//         }
//     }

//     public void StartInsectMovement(GameObject newFoodObject)
//     {
//         foodObject = newFoodObject;
//         isInsectMoving = true;
//         Debug.Log("지흔: Insect가 Food로 이동을 시작합니다.");
//     }

//     private void MoveInsectTowardsFood()
//     {
//         if (insectAnimator != null)
//         {
//             // 이동 중일 때 walk 애니메이션 활성화
//             insectAnimator.SetBool("walk", true);
//             insectAnimator.SetBool("idle", false);
//         }

//         float step = 0.5f * Time.deltaTime; // 이동 속도 조정

//         // 이동 방향 계산
//         Vector3 direction = (foodObject.transform.position - transform.position).normalized;

//         // Insect의 회전 설정 (부드럽게 회전)
//         Quaternion targetRotation = Quaternion.LookRotation(direction);
//         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

//         // Insect 오브젝트의 위치를 Food 오브젝트의 위치로 이동
//         transform.position = Vector3.MoveTowards(transform.position, foodObject.transform.position, step);

//         // 이동 후 Insect와 Food의 거리가 충분히 가까워졌을 때 충돌로 간주
//         if (Vector3.Distance(transform.position, foodObject.transform.position) < 0.1f)
//         {
//             Debug.Log("지흔: Food를 먹었습니다!");
//             // Food 제거 및 상태 업데이트
//             Destroy(foodObject); // Food 제거
//             isInsectMoving = false; // 이동 중지
//             foodObject = null; // Food 참조 제거

//             // 이동 중지 후 idle 애니메이션으로 전환
//             if (insectAnimator != null)
//             {
//                 insectAnimator.SetBool("walk", false);
//                 insectAnimator.SetBool("idle", true);
//             }
//         }
//     }
// }
