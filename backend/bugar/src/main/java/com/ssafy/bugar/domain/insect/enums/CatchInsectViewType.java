package com.ssafy.bugar.domain.insect.enums;

import com.ssafy.bugar.global.exception.CustomException;
import org.springframework.http.HttpStatus;

public enum CatchInsectViewType {
    CATCHED, RAISING, DONE;

    public static CatchInsectViewType fromString(String value) {
        try {
            return CatchInsectViewType.valueOf(value.toUpperCase());
        } catch (IllegalArgumentException e) {
            throw new CustomException(HttpStatus.BAD_REQUEST, "viewType을 다시 확인해주세요. (CATCHED, RAISING, DONE 만 가능)");
        }
    }
}
