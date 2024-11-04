package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
public class RaisingInsectService {

    private final RaisingInsectRepository raisingInsectRepository;

    @Transactional
    public void save(Long userId, Long insectId, String nickname) {
        RaisingInsect raisingInsect = RaisingInsect.builder()
                .userId(userId)
                .insectId(insectId)
                .insectNickname(nickname)
                .build();

        raisingInsectRepository.save(raisingInsect);
    }

}
