package com.ssafy.bugar.notification;

import com.ssafy.bugar.client.FcmClient;
import com.ssafy.bugar.dto.FcmMessageRequestDto;
import com.ssafy.bugar.notification.TokenService;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
public class FcmService {

    private final FcmClient fcmClient;
    private final TokenService tokenService;

    @Value("${fcm.server.key}")
    private String serverKey;

    public FcmService(FcmClient fcmClient, TokenService tokenService) {
        this.fcmClient = fcmClient;
        this.tokenService = tokenService;
    }

    public String sendNotificationToAll(FcmMessageRequestDto request) {
        StringBuilder responseLog = new StringBuilder();

        for (String token : tokenService.getAllTokens()) {
            String payload = "{"
                    + "\"to\": \"" + token + "\","
                    + "\"notification\": {"
                    + "\"title\": \"" + request.getTitle() + "\","
                    + "\"body\": \"" + request.getBody() + "\""
                    + "}"
                    + "}";

            String response = String.valueOf(
                    fcmClient.sendNotification("key=" + serverKey, "application/json", payload));
            responseLog.append("Response for token ").append(token).append(": ").append(response).append("\n");
        }

        return responseLog.toString();
    }
}
