package com.ssafy.bugar.domain.insect.dto.response;

import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Getter;

@Getter
@AllArgsConstructor
public class GetAreaInsectResponseDto {

    private int num;
    private List<InsectList> insectList;

    public interface InsectList {
        Long getRaisingInsectId();
        String getNickname();
        String getFamily();
    }
}
