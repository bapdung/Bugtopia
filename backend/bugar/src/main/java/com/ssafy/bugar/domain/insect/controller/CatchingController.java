package com.ssafy.bugar.domain.insect.controller;

import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.service.CatchingInsectService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@CrossOrigin
@RestController
@RequestMapping("/api/catch")
@RequiredArgsConstructor
public class CatchingController {

    private final CatchingInsectService catchingInsectService;

    @PostMapping
    public ResponseEntity<Void> saveCatchingInsect(@RequestHeader("userId") Long userId, @RequestBody CatchSaveRequestDto request) {
        catchingInsectService.save(userId, request);
        return ResponseEntity.status(HttpStatus.CREATED).build();
    }
}
