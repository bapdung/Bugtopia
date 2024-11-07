package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.response.SaveRaisingInsectResponseDto;
import com.ssafy.bugar.domain.insect.entity.Egg;
import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import com.ssafy.bugar.domain.insect.enums.RaiseState;
import com.ssafy.bugar.domain.insect.repository.EggRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import com.ssafy.bugar.domain.notification.enums.NotificationType;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Slf4j
@RequiredArgsConstructor
public class EggService {

    private final EggRepository eggRepository;
    private final RaisingInsectRepository raisingInsectRepository;
    private final RaisingInsectService raisingInsectService;

    @Transactional
    public void save(Long raisingInsectId, NotificationType type) {
        if (!validate(raisingInsectId, type)) {
            return;
        }
        RaisingInsect raisingInsect = raisingInsectRepository.findById(raisingInsectId).orElseThrow();
        Egg egg = new Egg(raisingInsect.getInsectNickname(), raisingInsect.getInsectId(), raisingInsect.getUserId());
        eggRepository.save(egg);
    }

    public Boolean validate(Long raisingInsectId, NotificationType type) {
        if (type != NotificationType.BABY) {
            return false;
        }
        RaisingInsect insect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);
        return insect.getState() == RaiseState.DONE;
    }

    @Transactional
    public SaveRaisingInsectResponseDto raise(Long eggId, Long userId, String nickname) {
        Egg egg = eggRepository.findByEggId(eggId);

        SaveRaisingInsectResponseDto response = raisingInsectService.save(userId, egg.getInsectId(), nickname);
        egg.raise();
        return response;
    }
}
