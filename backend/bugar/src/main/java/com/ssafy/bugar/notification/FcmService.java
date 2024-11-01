package com.ssafy.bugar.notification;

import com.ssafy.bugar.client.FcmClient;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
public class FcmService {
    private final FcmClient fcmClient;
    private final TokenService tokenService;

    @Value("${firebase.server.key}")
    private String serverKey;

    public FcmService(FcmClient fcmClient, TokenService tokenService) {
        this.fcmClient = fcmClient;
        this.tokenService = tokenService;
    }

    public void sendNotificationToAll(String title, String body) {
        for (String token : tokenService.getAllTokens()) {
            String payload = "{"
                    + "\"to\": \"" + token + "\","
                    + "\"notification\": {"
                    + "\"title\": \"" + title + "\","
                    + "\"body\": \"" + body + "\""
                    + "}"
                    + "}";
            fcmClient.sendNotification("key=" + serverKey, "application/json", payload);
        }
    }

}
