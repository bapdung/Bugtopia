package com.ssafy.bugar.domain.insect.controller;

import com.ssafy.bugar.domain.insect.dto.request.CatchDeleteRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchListResponseDto;
import com.ssafy.bugar.domain.insect.enums.CatchInsectViewType;
import com.ssafy.bugar.domain.insect.service.CatchingInsectService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@CrossOrigin
@RestController
@RequestMapping("/api/catch")
@RequiredArgsConstructor
@Slf4j
public class CatchingController {

    private final CatchingInsectService catchingInsectService;

    @PostMapping
    public ResponseEntity<Void> saveCatchingInsect(@RequestHeader("userId") Long userId, @RequestBody CatchSaveRequestDto request) {
        catchingInsectService.save(userId, request);
        return ResponseEntity.status(HttpStatus.CREATED).build();
    }

    @GetMapping
    public ResponseEntity<CatchListResponseDto> getCatchingInsectList(@RequestHeader("userId") Long userId, @RequestParam("viewType")
                                                                      CatchInsectViewType viewType) {
        log.info("첫 호출" + userId + ", " + viewType);
        CatchListResponseDto response = catchingInsectService.getCatchList(userId, viewType);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping
    public ResponseEntity<Void> deleteCatchingInsect(@RequestBody CatchDeleteRequestDto request) {
        catchingInsectService.deleteCatchInsect(request);
        return ResponseEntity.status(HttpStatus.ACCEPTED).build();
    }
}
