package com.ssafy.bugar.domain.insect.dto.response;

import com.ssafy.bugar.domain.insect.enums.AreaType;
import java.sql.Timestamp;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@NoArgsConstructor
public class GetInsectInfoResponseDto {

    private AreaType areaType;
    private int canFeed;
    private String nickname;
    private String insectName;
    private Long insectId;
    private Timestamp livingDate;
    private Long clearEvent;

    @Builder
    public GetInsectInfoResponseDto(AreaType areaType, int canFeed, String nickname, String insectName, Long insectId, Timestamp livingDate, Long clearEvent) {
        this.areaType = areaType;
        this.canFeed = canFeed;
        this.nickname = nickname;
        this.insectName = insectName;
        this.insectId = insectId;
        this.livingDate = livingDate;
        this.clearEvent = clearEvent;
    }

}
