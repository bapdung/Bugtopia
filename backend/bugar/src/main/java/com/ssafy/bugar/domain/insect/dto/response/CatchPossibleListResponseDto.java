package com.ssafy.bugar.domain.insect.dto.response;

import java.util.Collections;

import lombok.*;

import java.util.List;

@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@ToString
public class CatchPossibleListResponseDto {

    private int possibleInsectCnt;
    private int eggCnt;
    private List<PossibleInsect> possibleList = Collections.emptyList();
    private List<EggItem> eggList = Collections.emptyList();

    public interface PossibleInsect {
        Long getCatchedInsectId();
        String getPhoto();
        String getCatchedDate();
        String getInsectName();
    }

    @Getter
    @NoArgsConstructor
    @AllArgsConstructor
    @Builder
    public static class EggItem {
        private Long eggId;
        private String eggName;
        private String receiveDate;
    }
}