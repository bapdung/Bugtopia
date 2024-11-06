package com.ssafy.bugar.domain.insect.dto.response;

import com.ssafy.bugar.domain.insect.dto.response.CatchPossibleListResponseDto.PossibleInsect;
import java.util.Collections;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Builder.Default;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@AllArgsConstructor
@Builder
@NoArgsConstructor
public class CatchDoneListResponseDto {
    private int totalCnt;

    @Builder.Default
    private List<DoneInsectItem> doneList = Collections.emptyList();

    @Getter
    @AllArgsConstructor
    @Builder
    @NoArgsConstructor
    public static class DoneInsectItem {
        private Long raisingInsectId;
        private String family;
        private String insectNickname;
        private String doneDate;
    }
}
