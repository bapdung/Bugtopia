package com.ssafy.bugar.domain.insect.dto.request;

import lombok.Getter;
import lombok.NoArgsConstructor;

@NoArgsConstructor
@Getter
public class EggRaiseRequestDto {
    private String nickname;

    public EggRaiseRequestDto(String nickname) {
        this.nickname = nickname;
    }
}
