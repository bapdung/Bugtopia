package com.ssafy.bugar.notification;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class NotificationController {
    private final FcmService fcmService;

    public NotificationController(FcmService fcmService) {
        this.fcmService = fcmService;
    }

    @GetMapping("/send-notification")
    public String sendNotification(@RequestParam String title, @RequestParam String body) {
        fcmService.sendNotificationToAll(title, body);
        return "알림 전송 완료!";
    }
}
