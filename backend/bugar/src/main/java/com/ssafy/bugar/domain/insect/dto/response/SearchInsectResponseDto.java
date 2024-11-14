package com.ssafy.bugar.domain.insect.dto.response;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class SearchInsectResponseDto {

    private int status;
    private Content content;

    @Getter
    @Builder
    public static class Content {
        private long insectId;
        private String krName;
        private String engName;
        private String info;
        private String imgUrl;
        private int canRaise;
        private String family;
        private String area;
        private String rejectedReason;
    }

}
