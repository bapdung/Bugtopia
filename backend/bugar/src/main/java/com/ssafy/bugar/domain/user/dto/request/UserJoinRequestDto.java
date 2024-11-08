package com.ssafy.bugar.domain.user.dto.request;

import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@NoArgsConstructor
@Getter
public class UserJoinRequestDto {
    private String deviceId;
    private String nickname;

    @Builder
    public UserJoinRequestDto(String deviceId, String nickname) {
        this.deviceId = deviceId;
        this.nickname = nickname;
    }
}
