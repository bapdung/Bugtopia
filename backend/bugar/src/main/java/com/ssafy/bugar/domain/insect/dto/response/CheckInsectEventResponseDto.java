package com.ssafy.bugar.domain.insect.dto.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.ssafy.bugar.domain.insect.enums.EventType;
import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class CheckInsectEventResponseDto {

    private int loveScore;
    private boolean isEvent;
    private EventType eventType;

    @JsonProperty("isEvent")
    public boolean getIsEvent() {
        return isEvent;
    }

}
