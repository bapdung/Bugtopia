package com.ssafy.bugar.domain.insect.controller;

import com.ssafy.bugar.domain.insect.dto.request.SaveLoveScoreRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.SaveRaisingInsectRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CheckInsectEventResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetInsectInfoResponseDto;
import com.ssafy.bugar.domain.insect.service.RaisingInsectService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
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
        raisingInsectService.saveLoveScore(saveLoveScoreRequestDto.getRaisingInsectId(), saveLoveScoreRequestDto.getCategory());
        return ResponseEntity.status(HttpStatus.CREATED).build();
    }

    @GetMapping("/area")
    public ResponseEntity<GetAreaInsectResponseDto> getAreaInsect(@RequestHeader("userId") Long userId, @RequestParam String areaType) {
        GetAreaInsectResponseDto getAreaInsectResponseDto = raisingInsectService.searchAreaInsect(userId, areaType);
        return ResponseEntity.ok(getAreaInsectResponseDto);
    }

    @GetMapping("/{raisingInsectId}")
    public ResponseEntity<GetInsectInfoResponseDto> getInsectInfo(@PathVariable Long raisingInsectId) {
        GetInsectInfoResponseDto getInsectInfoResponseDto = raisingInsectService.search(raisingInsectId);
        return ResponseEntity.ok(getInsectInfoResponseDto);
    }

    @GetMapping("/event/{raisingInsectId}")
    public ResponseEntity<CheckInsectEventResponseDto> checkInsectEvent(@PathVariable Long raisingInsectId) {
        return ResponseEntity.ok(raisingInsectService.checkInsectEvent(raisingInsectId));
    }

}
