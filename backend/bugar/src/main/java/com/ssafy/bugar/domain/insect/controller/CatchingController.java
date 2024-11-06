package com.ssafy.bugar.domain.insect.controller;

import com.ssafy.bugar.domain.insect.dto.request.CatchDeleteRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectDetailResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchListResponseDto;
import com.ssafy.bugar.domain.insect.enums.CatchInsectDetailViewType;
import com.ssafy.bugar.domain.insect.enums.CatchInsectViewType;
import com.ssafy.bugar.domain.insect.service.CatchingInsectService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
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
        CatchListResponseDto response = catchingInsectService.getCatchList(userId, viewType);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping
    public ResponseEntity<Void> deleteCatchingInsect(@RequestBody CatchDeleteRequestDto request) {
        catchingInsectService.deleteCatchInsect(request);
        return ResponseEntity.status(HttpStatus.ACCEPTED).build();
    }

    @GetMapping("/{insectId}")
    public ResponseEntity<CatchInsectDetailResponseDto> getInsectDetail(@RequestHeader("userId") Long userId, @PathVariable Long insectId,
                                                                        @RequestParam CatchInsectDetailViewType viewType) {
        CatchInsectDetailResponseDto response = catchingInsectService.getDetail(insectId, viewType, userId);
        return ResponseEntity.ok(response);
    }
}
