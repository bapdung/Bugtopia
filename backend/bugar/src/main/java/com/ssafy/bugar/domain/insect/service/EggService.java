package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.response.SaveRaisingInsectResponseDto;
import com.ssafy.bugar.domain.insect.entity.Egg;
import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import com.ssafy.bugar.domain.insect.enums.RaiseState;
import com.ssafy.bugar.domain.insect.repository.EggRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import com.ssafy.bugar.domain.notification.dto.response.NotificationResponseDto;
import com.ssafy.bugar.domain.notification.enums.NotificationType;
import com.ssafy.bugar.domain.notification.service.NotificationService;
import com.ssafy.bugar.global.exception.CustomException;
import java.io.IOException;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Slf4j
@RequiredArgsConstructor
public class EggService {

    private final EggRepository eggRepository;
    private final RaisingInsectRepository raisingInsectRepository;
    private final RaisingInsectService raisingInsectService;
    private final NotificationService notificationService;

    // 알 생성
    @Transactional
    public NotificationResponseDto save(Long raisingInsectId) throws IOException {
        if (!validate(raisingInsectId)) {
            throw new CustomException(HttpStatus.BAD_REQUEST, "알을 낳을 수 있는 곤충인지 다시 확인해주세요.");
        }
        RaisingInsect raisingInsect = raisingInsectRepository.findById(raisingInsectId).orElseThrow();
        Egg egg = new Egg(raisingInsect.getInsectNickname(), raisingInsect.getInsectId(), raisingInsect.getUserId());
        eggRepository.save(egg);
        return notificationService.save(raisingInsectId, NotificationType.BABY);
    }

    // 알 낳을 수 있는 곤충인지 판별
    public Boolean validate(Long raisingInsectId) {
        RaisingInsect insect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);
        return insect.getState() == RaiseState.DONE;
    }

    // 알 -> 키우기
    @Transactional
    public SaveRaisingInsectResponseDto raise(Long eggId, Long userId, String nickname) {
        Egg egg = eggRepository.findByEggId(eggId);

        SaveRaisingInsectResponseDto response = raisingInsectService.save(userId, egg.getInsectId(), nickname);
        egg.raise();
        return response;
    }
}
