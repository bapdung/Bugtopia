package com.ssafy.bugar.client;

import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;

@FeignClient(name = "fcmClient", url = "https://fcm.googleapis.com/fcm")
public interface FcmClient {
    @PostMapping("/send")
    ResponseEntity<String> sendNotification(
            @RequestHeader("Authorization") String authorization,
            @RequestHeader("Content-Type") String contentType,
            @RequestBody String payload);
}
