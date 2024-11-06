package com.ssafy.bugar.domain.insect.dto.request;

import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class SaveRaisingInsectRequestDto {

    private Long insectId;
    private String nickname;

}
