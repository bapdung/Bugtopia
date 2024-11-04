package com.ssafy.bugar.domain.insect.controller;

import com.ssafy.bugar.domain.insect.dto.request.SaveLoveScoreRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.SaveRaisingInsectRequestDto;
import com.ssafy.bugar.domain.insect.service.RaisingInsectService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@CrossOrigin
@RestController
@RequestMapping("/api/insect")
@RequiredArgsConstructor
public class RaisingController {

    private final RaisingInsectService raisingInsectService;

    @PostMapping
    public ResponseEntity<Void> saveRaisingInsect(@RequestHeader("userId") Long userId, @RequestBody SaveRaisingInsectRequestDto saveRaisingInsectRequestDto) {
        raisingInsectService.save(userId, saveRaisingInsectRequestDto.getInsectId(),
                saveRaisingInsectRequestDto.getNickname());
        return ResponseEntity.status(HttpStatus.CREATED).build();
    }

    @PostMapping("/love-score")
    public ResponseEntity<Void> saveLoveScore(@RequestBody SaveLoveScoreRequestDto saveLoveScoreRequestDto) {
        raisingInsectService.saveLoveScore(saveLoveScoreRequestDto.getInsectId(), saveLoveScoreRequestDto.getCategory());
        return ResponseEntity.status(HttpStatus.CREATED).build();
    }

}
