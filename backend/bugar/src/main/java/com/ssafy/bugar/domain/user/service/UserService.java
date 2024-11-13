package com.ssafy.bugar.domain.user.service;

import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import com.ssafy.bugar.domain.user.dto.request.UserJoinRequestDto;
import com.ssafy.bugar.domain.user.dto.request.UserLoginRequestDto;
import com.ssafy.bugar.domain.user.dto.response.UserLoginResponseDto;
import com.ssafy.bugar.domain.user.entity.User;
import com.ssafy.bugar.domain.user.repository.UserRepository;
import jakarta.transaction.Transactional;
import java.util.ArrayList;
import java.util.List;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

@Service
@Transactional
@RequiredArgsConstructor
@Slf4j
public class UserService {

    private final UserRepository userRepository;
    private final RaisingInsectRepository raisingInsectRepository;

    public User join(UserJoinRequestDto request) {
        log.info(request.getDeviceId() + "@@@@@@@@@@기기정보");
        log.info(request.getNickname() + "@@@@@@@@@@닉네임");
        User user = User.builder()
                .deviceId(request.getDeviceId())
                .nickname(request.getNickname())
                .build();

        return userRepository.save(user);
    }

    public UserLoginResponseDto login(UserLoginRequestDto request) {
        User user = userRepository.findByDeviceId(request.getDeviceId());
        if (user == null) {
            return UserLoginResponseDto.builder()
                    .isJoined(false)
                    .build();
        }
        List<RaisingInsect> insectList = raisingInsectRepository.findAllByUserId(user.getUserId());
        List<Long> insects = new ArrayList<>();

        for (RaisingInsect insect : insectList) {
            insects.add(insect.getRaisingInsectId());
        }
        return UserLoginResponseDto.builder()
                .isJoined(true)
                .userId(user.getUserId())
                .nickname(user.getNickname())
                .raisingInsects(insects)
                .build();
    }
}