package com.ssafy.bugar.domain.insect.dto.request;

import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class SaveLoveScoreRequestDto {

    private Long raisingInsectId;
    private int category;

}
