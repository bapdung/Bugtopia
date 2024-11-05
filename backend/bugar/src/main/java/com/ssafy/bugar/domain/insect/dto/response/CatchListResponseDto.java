package com.ssafy.bugar.domain.insect.dto.response;

import com.ssafy.bugar.domain.insect.enums.CatchState;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class CatchListResponseDto {
    private int totalCnt;
    private List<InsectItem> InsectList;

    @Builder
    @AllArgsConstructor
    public static class InsectItem {
        private String insectName;
        private String photo;
        private String catchedDate;
        private CatchState state;
    }
}
