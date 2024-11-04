package com.ssafy.bugar.notification;

import com.ssafy.bugar.dto.FcmMessageRequestDto;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@Slf4j
public class NotificationController {

    private final FcmService fcmService;

    public NotificationController(FcmService fcmService) {
        this.fcmService = fcmService;
    }

    @PostMapping("/test")
    public ResponseEntity<String> sendNotification(@RequestBody FcmMessageRequestDto request) {
        String response = fcmService.sendNotificationToAll(request);
        return ResponseEntity.ok(response);
    }

    @PostMapping("/save-token")
    public ResponseEntity<String> saveToken(@RequestParam String fcmToken) {
        // FCM 토큰을 저장하는 서비스 로직 호출
        log.info(fcmToken + "@@@@@@@@@@토큰");
        return ResponseEntity.ok(fcmToken);
    }
}