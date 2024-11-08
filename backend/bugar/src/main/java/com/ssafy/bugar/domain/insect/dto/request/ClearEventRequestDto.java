package com.ssafy.bugar.domain.insect.dto.request;

import com.ssafy.bugar.domain.insect.enums.EventType;
import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class ClearEventRequestDto {

    private long raisingInsectId;
    private EventType clearEventType;

}
