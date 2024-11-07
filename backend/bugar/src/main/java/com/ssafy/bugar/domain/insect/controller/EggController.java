package com.ssafy.bugar.domain.insect.controller;

import com.ssafy.bugar.domain.insect.dto.request.EggRaiseRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.SaveRaisingInsectResponseDto;
import com.ssafy.bugar.domain.insect.service.EggService;
import lombok.RequiredArgsConstructor;
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
@RequestMapping("/api/egg")
@RequiredArgsConstructor
public class EggController {

    private final EggService eggService;

    @PostMapping("/egg/{eggId}")
    public ResponseEntity<SaveRaisingInsectResponseDto> receiveEgg(@RequestHeader("userId") Long userId,
                                                                   @PathVariable Long eggId,
                                                                   @RequestBody EggRaiseRequestDto request) {
        SaveRaisingInsectResponseDto response = eggService.raise(eggId, userId, request.getNickname());
        return ResponseEntity.ok(response);
    }
}
