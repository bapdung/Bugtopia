package com.ssafy.bugar.domain.user.controller;

import com.ssafy.bugar.domain.user.dto.request.UserJoinRequestDto;
import com.ssafy.bugar.domain.user.dto.response.UserJoinResponseDto;
import com.ssafy.bugar.domain.user.entity.User;
import com.ssafy.bugar.domain.user.service.UserService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@CrossOrigin
@RequestMapping("/api/user")
@RequiredArgsConstructor
public class UserController {

    private final UserService userService;

    @PostMapping("/join")
    public ResponseEntity<UserJoinResponseDto> join(@RequestBody UserJoinRequestDto request) {
        User user = userService.join(request);
        UserJoinResponseDto response = UserJoinResponseDto.builder()
                .user(user)
                .build();

        return ResponseEntity.ok(response);
    }
}