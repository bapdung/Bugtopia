package com.ssafy.bugar.domain.insect.dto.request;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@AllArgsConstructor
@NoArgsConstructor
public class CatchDeleteRequestDto {
    private Long catchedInsectId;
}
