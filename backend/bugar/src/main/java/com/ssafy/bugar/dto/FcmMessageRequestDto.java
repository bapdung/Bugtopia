package com.ssafy.bugar.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@AllArgsConstructor
@NoArgsConstructor
public class FcmMessageRequestDto {
    private String title;
    private String body;

}
