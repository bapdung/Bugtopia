package com.ssafy.bugar.domain.insect.dto.response;

import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class SaveRaisingInsectResponseDto {

    private Long raisingInsectId;
    private String nickname;
    private String family;

}
