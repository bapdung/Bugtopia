package com.ssafy.bugar.domain.insect.dto.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.ssafy.bugar.domain.insect.enums.AreaType;
import com.ssafy.bugar.domain.insect.enums.EventType;
import java.sql.Timestamp;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class GetInsectInfoResponseDto {

    private Long raisingInsectId;
    private String nickname;
    private String insectName;
    private String family;
    private AreaType areaType;
    private int feedCnt;
    private Timestamp lastEat;
    private int interactCnt;
    private Timestamp livingDate;
    private int continuousDays;
    private int loveScore;
    private boolean isEvent;  // 그대로 유지합니다
    private EventType eventType;

    @JsonProperty("isEvent")
    public boolean getIsEvent() {
        return isEvent;
    }
}

