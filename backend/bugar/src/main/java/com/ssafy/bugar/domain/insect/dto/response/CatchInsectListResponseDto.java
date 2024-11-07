package com.ssafy.bugar.domain.insect.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto.InsectList;
import java.util.Collections;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@JsonInclude(JsonInclude.Include.NON_NULL)
@Getter
@AllArgsConstructor
@Builder
public class CatchInsectListResponseDto {

    // 채집 곤충 필드
    private Integer catchedInsectCnt;
    private Integer eggCnt; // 알의 수
    private List<CatchedInsectItem> catchList = Collections.emptyList(); // 채집한 곤충 목록
    private List<EggItem> eggList = Collections.emptyList(); // 알 목록

    // 육성중 탭 필드
    private Integer forestCnt; // 숲에 있는 곤충 수
    private Integer waterCnt; // 물에 있는 곤충 수
    private Integer gardenCnt; // 정원에 있는 곤충 수
    private List<InsectList> forestList = Collections.emptyList(); // 숲의 곤충 목록
    private List<InsectList> waterList = Collections.emptyList(); // 물의 곤충 목록
    private List<InsectList> gardenList = Collections.emptyList(); // 정원의 곤충 목록

    // 육성완료 탭 필드
    private Integer totalCnt; // 육성 완료된 총 곤충 수
    private List<DoneInsectItem> doneList = Collections.emptyList(); // 완료된 곤충 목록

    // 인터페이스(알)
    public interface EggItem {
        Long getEggId();

        String getEggName();

        String getReceiveDate();
    }

    // 인터페이스 (채집곤충)
    public interface CatchedInsectItem {
        Long getCatchedInsectId();

        String getPhoto();

        String getCatchedDate();

        String getInsectName();
    }

    public interface DoneInsectItem {
        Long getRaisingInsectId();

        String getFamily();

        String getInsectNickname();

        String getDoneDate();

        int getMessageCnt();
    }

}
