package com.ssafy.bugar.domain.insect.enums;

import com.ssafy.bugar.global.exception.CustomException;
import org.springframework.http.HttpStatus;

public enum CatchInsectDetailViewType {
    CATCHED, DONE;

    public static CatchInsectDetailViewType fromString(String value) {
        try {
            return CatchInsectDetailViewType.valueOf(value.toUpperCase());
        } catch (IllegalArgumentException e) {
            throw new CustomException(HttpStatus.BAD_REQUEST, "viewType을 다시 확인해주세요. (CATCHED, DONE 만 가능)");
        }
    }
}
