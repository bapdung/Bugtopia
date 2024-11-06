package com.ssafy.bugar.domain.insect.dto.response;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.util.List;

@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class CatchPossibleListResponseDto {

    private int totalCnt;
    private List<PossibleInsect> possibleList;

    @Getter
    @NoArgsConstructor
    @AllArgsConstructor
    @Builder
    public static class PossibleInsect {
        private Long catchedInsectId;
        private String insectName;
        private String photo;
        private String catchedDate;
    }
}