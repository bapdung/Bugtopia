package com.ssafy.bugar.domain.notification.dto.response;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Builder
@Getter
@AllArgsConstructor
public class NotificationResponseDto {
    private Long userId;
    private String message;
    private String userNickname;
}
