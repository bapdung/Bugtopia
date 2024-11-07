package com.ssafy.bugar.domain.user.dto.request;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@Builder
@AllArgsConstructor
public class UserJoinRequestDto {
    private String deviceId;
    private String nickname;
}
