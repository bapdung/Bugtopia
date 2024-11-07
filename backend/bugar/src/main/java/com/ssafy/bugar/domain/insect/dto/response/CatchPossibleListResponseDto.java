package com.ssafy.bugar.domain.insect.dto.response;

import java.util.Collections;

import lombok.*;

import java.util.List;

@Getter
@AllArgsConstructor
@Builder
public class CatchPossibleListResponseDto {

    private int possibleInsectCnt;
    private int eggCnt;
    private List<EggItem> eggList = Collections.emptyList();
    private List<PossibleInsect> possibleList = Collections.emptyList();

    public interface EggItem {
        Long getEggId();
        String getEggName();
        String getReceiveDate();
    }

    public interface PossibleInsect {
        Long getCatchedInsectId();
        String getPhoto();
        String getCatchedDate();
        String getInsectName();
    }
}