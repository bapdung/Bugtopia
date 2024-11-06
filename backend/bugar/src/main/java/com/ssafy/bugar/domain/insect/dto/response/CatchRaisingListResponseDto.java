package com.ssafy.bugar.domain.insect.dto.response;

import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto.InsectList;
import java.util.Collections;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class CatchRaisingListResponseDto {
    private int forestCnt;
    private int waterCnt;
    private int gardenCnt;

    @Builder.Default
    private List<InsectList> forestList = Collections.emptyList();

    @Builder.Default
    private List<InsectList> waterList = Collections.emptyList();

    @Builder.Default
    private List<InsectList> gardenList = Collections.emptyList();

}
