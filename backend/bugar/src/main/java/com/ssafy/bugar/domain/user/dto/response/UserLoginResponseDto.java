package com.ssafy.bugar.domain.user.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@AllArgsConstructor
@Builder
@JsonInclude(JsonInclude.Include.NON_NULL)
public class UserLoginResponseDto {
    private boolean isJoined;
    private Long userId;
    private String nickname;
    private List<Long> raisingInsects;
}
