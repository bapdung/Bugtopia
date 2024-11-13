package com.ssafy.bugar.domain.notification.dto.request;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Builder
@AllArgsConstructor
@Getter
public class PushMessageRequestDto {
    private boolean validateOnly;
    private Message message;

    @Builder
    @AllArgsConstructor
    @Getter
    public static class Message {
        private Data data;
        private String token;
    }

    @Builder
    @AllArgsConstructor
    @Getter
    public static class Data {
        private String title;
        private String body;
        private String image;
    }
}
