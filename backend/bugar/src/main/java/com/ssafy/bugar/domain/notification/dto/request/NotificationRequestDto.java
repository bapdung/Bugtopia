package com.ssafy.bugar.domain.notification.dto.request;

import lombok.Data;

@Data
public class NotificationRequestDto {
    private String targetToken;
    private String title;
    private String body;
}
