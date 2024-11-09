package com.ssafy.bugar.domain.insect.controller;

import com.ssafy.bugar.domain.insect.dto.request.ClearEventRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.SaveLoveScoreRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.SaveRaisingInsectRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CheckInsectEventResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetArInsectInfoResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetInsectInfoResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.SaveRaisingInsectResponseDto;
import com.ssafy.bugar.domain.insect.service.RaisingInsectService;
import java.io.IOException;
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
    public ResponseEntity<SaveRaisingInsectResponseDto> saveRaisingInsect(
            @RequestHeader("userId") Long userId,
            @RequestBody SaveRaisingInsectRequestDto saveRaisingInsectRequestDto) {
        SaveRaisingInsectResponseDto saveRaisingInsectResponseDto = raisingInsectService.save(
                userId,
                saveRaisingInsectRequestDto.getInsectId(),
                saveRaisingInsectRequestDto.getNickname()
        );
        return ResponseEntity.ok(saveRaisingInsectResponseDto);
    }

    @PostMapping("/love-score")
    public ResponseEntity<CheckInsectEventResponseDto> saveLoveScore(
            @RequestBody SaveLoveScoreRequestDto saveLoveScoreRequestDto) throws IOException {
        CheckInsectEventResponseDto checkInsectEventResponseDto = raisingInsectService.saveLoveScore(
                saveLoveScoreRequestDto.getRaisingInsectId(), saveLoveScoreRequestDto.getCategory());
        return ResponseEntity.ok(checkInsectEventResponseDto);
    }

    @GetMapping("/area")
    public ResponseEntity<GetAreaInsectResponseDto> getAreaInsect(@RequestHeader("userId") Long userId,
                                                                  @RequestParam String areaType) {
        return ResponseEntity.ok(raisingInsectService.searchAreaInsect(userId, areaType));
    }

    @GetMapping("/{raisingInsectId}")
    public ResponseEntity<GetInsectInfoResponseDto> getInsectInfo(@PathVariable Long raisingInsectId)
            throws IOException {
        return ResponseEntity.ok(raisingInsectService.search(raisingInsectId));
    }

    @GetMapping("/event/{raisingInsectId}")
    public ResponseEntity<CheckInsectEventResponseDto> checkInsectEvent(@PathVariable Long raisingInsectId)
            throws IOException {
        return ResponseEntity.ok(raisingInsectService.checkInsectEvent(raisingInsectId));
    }

    @PostMapping("/event/clear")
    public ResponseEntity<Void> clearEvent(@RequestBody ClearEventRequestDto clearEventRequestDto) {
        raisingInsectService.clearEvent(clearEventRequestDto.getRaisingInsectId(),
                clearEventRequestDto.getClearEventType());
        return ResponseEntity.status(HttpStatus.OK).build();
    }

    @PostMapping("/release/{raisingInsectId}")
    public ResponseEntity<Void> release(@PathVariable("raisingInsectId") Long raisingInsectId) {
        raisingInsectService.release(raisingInsectId);
        return ResponseEntity.status(HttpStatus.OK).build();
    }

    @GetMapping("/ar/{raisingInsectId}")
    public ResponseEntity<GetArInsectInfoResponseDto> getInsectArInfo(@PathVariable Long raisingInsectId) {
        return ResponseEntity.ok(raisingInsectService.getInsectArInfo(raisingInsectId));
    }
}
