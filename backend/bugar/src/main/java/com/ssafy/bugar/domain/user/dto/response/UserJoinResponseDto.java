package com.ssafy.bugar.domain.user.dto.response;

import com.ssafy.bugar.domain.user.entity.User;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@NoArgsConstructor
@Getter
public final class UserJoinResponseDto {
    private Long userId;
    private String nickname;

    @Builder
    public UserJoinResponseDto(User user) {
        this.userId = user.getUserId();
        this.nickname = user.getNickname();
    }
}
