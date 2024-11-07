package com.ssafy.bugar.domain.insect.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import java.util.Collections;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@JsonInclude(JsonInclude.Include.NON_NULL)
@Getter
@AllArgsConstructor
@Builder
public class CatchListResponseDto {

    // 육성가능 탭 필드
    private Integer possibleInsectCnt; // 육성이 가능한 곤충의 수
    private Integer eggCnt; // 알의 수
    private List<CatchPossibleListResponseDto.PossibleInsect> possibleList = Collections.emptyList(); // 육성 가능한 곤충 목록
    private List<CatchPossibleListResponseDto.EggItem> eggList = Collections.emptyList(); // 알 목록

    // 육성중 탭 필드
    private Integer forestCnt; // 숲에 있는 곤충 수
    private Integer waterCnt; // 물에 있는 곤충 수
    private Integer gardenCnt; // 정원에 있는 곤충 수
    private List<GetAreaInsectResponseDto.InsectList> forestList = Collections.emptyList(); // 숲의 곤충 목록
    private List<GetAreaInsectResponseDto.InsectList> waterList = Collections.emptyList(); // 물의 곤충 목록
    private List<GetAreaInsectResponseDto.InsectList> gardenList = Collections.emptyList(); // 정원의 곤충 목록

    // 육성완료 탭 필드
    private Integer totalCnt; // 육성 완료된 총 곤충 수
    private List<CatchDoneListResponseDto.DoneInsectItem> doneList = Collections.emptyList(); // 완료된 곤충 목록
}
