package com.ssafy.bugar.domain.insect.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import java.util.Collections;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

// null인 필드는 JSON에서 보이지 않도록 설정
@JsonInclude(JsonInclude.Include.NON_NULL)
@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class CatchListResponseDto {

    // 육성가능 탭 필드
    private int possibleInsectCnt; // 육성이 가능한 곤충의 수
    private int eggCnt; // 알의 수

    @Builder.Default
    private List<CatchPossibleListResponseDto.PossibleInsect> possibleList = Collections.emptyList(); // 육성 가능한 곤충 목록

    @Builder.Default
    private List<CatchPossibleListResponseDto.EggItem> eggList = Collections.emptyList(); // 알 목록

    // 육성중 탭 필드
    private int forestCnt; // 숲에 있는 곤충 수
    private int waterCnt; // 물에 있는 곤충 수
    private int gardenCnt; // 정원에 있는 곤충 수

    @Builder.Default
    private List<GetAreaInsectResponseDto.InsectList> forestList = Collections.emptyList(); // 숲의 곤충 목록

    @Builder.Default
    private List<GetAreaInsectResponseDto.InsectList> waterList = Collections.emptyList(); // 물의 곤충 목록

    @Builder.Default
    private List<GetAreaInsectResponseDto.InsectList> gardenList = Collections.emptyList(); // 정원의 곤충 목록

    // 육성완료 탭 필드
    private int totalCnt; // 육성 완료된 총 곤충 수

    @Builder.Default
    private List<CatchDoneListResponseDto.DoneInsectItem> doneList = Collections.emptyList(); // 완료된 곤충 목록
}
