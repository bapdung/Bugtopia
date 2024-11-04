package com.ssafy.bugar.domain.insect.dto.response;

import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class GetAreaInsectResponseDto {

    private int num;
    private List<InsectList> insectListList;

    public interface InsectList {
        Long getInsectId();
        String getNickname();
    }
}
