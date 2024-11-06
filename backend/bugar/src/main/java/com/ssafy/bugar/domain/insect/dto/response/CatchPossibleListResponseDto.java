package com.ssafy.bugar.domain.insect.dto.response;

import java.util.Collections;
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

    private int possibleInsectCnt;
    private int eggCnt;

    @Builder.Default
    private List<PossibleInsect> possibleList = Collections.emptyList();

    @Builder.Default
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