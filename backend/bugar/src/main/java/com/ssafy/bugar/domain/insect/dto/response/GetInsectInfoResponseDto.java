package com.ssafy.bugar.domain.insect.dto.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.ssafy.bugar.domain.insect.enums.AreaType;
import com.ssafy.bugar.domain.insect.enums.EventType;
import java.sql.Timestamp;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class GetInsectInfoResponseDto {

    private Info info;
    private LoveScore loveScore;
    private Event event;

    @Getter
    @AllArgsConstructor
    @Builder
    public static class Info {
        private Long raisingInsectId;
        private String nickname;
        private String insectName;
        private String family;
        private AreaType areaType;
        private Timestamp livingDate;
    }

    @Getter
    @AllArgsConstructor
    @Builder
    public static class LoveScore {
        private int total;
        private int feedCnt;
        private Timestamp lastEat;
        private int interactCnt;
    }

    @Getter
    @AllArgsConstructor
    @Builder
    public static class Event {
        private EventType endEvent;
        private boolean isEvent;
        private EventType eventType;

        @JsonProperty("isEvent")
        public boolean getIsEvent() {
            return isEvent;
        }
    }
}

