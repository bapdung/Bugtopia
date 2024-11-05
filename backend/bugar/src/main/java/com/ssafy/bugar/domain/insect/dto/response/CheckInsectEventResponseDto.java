package com.ssafy.bugar.domain.insect.dto.response;

import com.ssafy.bugar.domain.insect.enums.EventType;
import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class CheckInsectEventResponseDto {

    private int loveScore;
    private boolean isEvnet;
    private EventType eventType;

}
