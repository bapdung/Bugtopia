package com.ssafy.bugar.global.exception;

import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
@Slf4j
public class CustomErrorHandler {

    @ExceptionHandler(CustomException.class)
    public ResponseEntity<CustomErrorResponse> handleException(CustomException e) {
        log.error("RuntimeException 발생 : " + e.getMessage());
        CustomErrorResponse errorResponse = new CustomErrorResponse(e.getMessage(), e.getStatus().value());
        return new ResponseEntity<>(errorResponse, e.getStatus());
    }
}
