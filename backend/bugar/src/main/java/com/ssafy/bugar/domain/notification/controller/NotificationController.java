package com.ssafy.bugar.domain.notification.controller;

import com.ssafy.bugar.domain.notification.dto.request.NotificationRequestDto;
import com.ssafy.bugar.domain.notification.dto.response.NotificationResponseDto;
import com.ssafy.bugar.domain.notification.service.NotificationService;
import java.io.IOException;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/notification")
@RequiredArgsConstructor
public class NotificationController {

    private final NotificationService notificationService;

    @PostMapping("/push")
    public ResponseEntity<NotificationResponseDto> pushMessage(@RequestBody NotificationRequestDto requestDTO)
            throws IOException {

        NotificationResponseDto response = notificationService.save(requestDTO.getRaisingInsectId(),
                requestDTO.getType());
        return ResponseEntity.ok(response);
    }

    @PatchMapping("/{notificationId}")
    public ResponseEntity<Void> readMessage(@PathVariable Long notificationId) {
        notificationService.readMessage(notificationId);
        return ResponseEntity.ok().build();
    }
}
