package com.ssafy.bugar.domain.insect.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.ssafy.bugar.domain.insect.enums.AreaType;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@NoArgsConstructor
@AllArgsConstructor
@JsonInclude(JsonInclude.Include.NON_NULL)
@Builder
public class CatchInsectDetailResponseDto {
    // 공통 속성
    private String krwName;
    private String family;

    // 채집 곤충
    private String engName;
    private String info;
    private Integer canRaise;
    private AreaType area;
    private String rejectedReason;

    // 육성 완료 곤충
    private String insectNickname;
    private String startDate;
    private String doneDate;
    private Integer meetingDays;

    public interface CatchInsectDetailProjection {

        // 공통 속성
        String getKrwName();
        String getFamily();

        // 채집 곤충
        String getEngName();
        String getInfo();
        Integer getCanRaise();
        AreaType getArea();
        String getRejectedReason();

        // 육성 완료 곤충
        String getInsectNickname();
        String getStartDate();
        String getDoneDate();
        Integer getMeetingDays();
    }
}
