package com.ssafy.bugar.domain.user.dto.request;

import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@NoArgsConstructor
public class UserLoginRequestDto {
    private String deviceId;

    public UserLoginRequestDto(String deviceId) {
        this.deviceId = deviceId;
    }
}
