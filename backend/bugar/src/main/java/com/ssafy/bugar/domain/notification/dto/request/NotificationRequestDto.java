package com.ssafy.bugar.domain.notification.dto.request;

import com.ssafy.bugar.domain.notification.enums.NotificationType;
import lombok.Data;

@Data
public class NotificationRequestDto {
    private Long raisingInsectId;
    private NotificationType type;
}
