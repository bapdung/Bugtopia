package com.ssafy.bugar.domain.insect.dto.response;

import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto.InsectList;
import java.util.Collections;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@AllArgsConstructor
@Builder
public class CatchRaisingListResponseDto {
    private int forestCnt;
    private int waterCnt;
    private int gardenCnt;
    private List<InsectList> forestList = Collections.emptyList();
    private List<InsectList> waterList = Collections.emptyList();
    private List<InsectList> gardenList = Collections.emptyList();
}
