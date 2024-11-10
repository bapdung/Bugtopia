package com.ssafy.bugar.domain.insect.dto.response;

import java.sql.Timestamp;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@AllArgsConstructor
@Builder
public class GetArInsectInfoResponseDto {

    private long raisingInsectId;
    private String nickname;
    private String family;
    private int feedCnt;
    private Timestamp lastEat;
    private int interactCnt;

}
