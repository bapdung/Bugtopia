package com.ssafy.bugar.domain.insect.dto.response;

import java.util.Collections;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@AllArgsConstructor
@Builder
public class CatchDoneListResponseDto {
    private int totalCnt;
    private List<DoneInsectItem> doneList = Collections.emptyList();

    @Getter
    @AllArgsConstructor
    @Builder
    public static class DoneInsectItem {
        private Long raisingInsectId;
        private String family;
        private String insectNickname;
        private String doneDate;
    }
}
