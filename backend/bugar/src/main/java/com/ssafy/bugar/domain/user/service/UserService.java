package com.ssafy.bugar.domain.user.service;

import com.ssafy.bugar.domain.user.dto.request.UserJoinRequestDto;
import com.ssafy.bugar.domain.user.dto.response.UserJoinResponseDto;
import com.ssafy.bugar.domain.user.entity.User;
import com.ssafy.bugar.domain.user.repository.UserRepository;
import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

@Service
@Transactional
@RequiredArgsConstructor
public class UserService {

    private final UserRepository userRepository;

    public User join(UserJoinRequestDto request) {
        User user = User.builder()
                .deviceId(request.getDeviceId())
                .nickname(request.getNickname())
                .build();

        return userRepository.save(user);
    }
}
