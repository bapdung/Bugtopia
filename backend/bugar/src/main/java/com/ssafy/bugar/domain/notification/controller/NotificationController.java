package com.ssafy.bugar.domain.notification.controller;

import com.ssafy.bugar.domain.notification.dto.request.NotificationRequestDto;
import com.ssafy.bugar.domain.notification.service.FirebaseService;
import java.io.IOException;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
public class NotificationController {

    private final FirebaseService firebaseService;

    @PostMapping("/api/fcm")
    public ResponseEntity pushMessage(@RequestBody NotificationRequestDto requestDTO) throws IOException {
        System.out.println(requestDTO.getTargetToken() + " "
                + requestDTO.getTitle() + " " + requestDTO.getBody());

        firebaseService.sendMessageTo(
                requestDTO.getTargetToken(),
                requestDTO.getTitle(),
                requestDTO.getBody());
        return ResponseEntity.ok().build();
    }
}
