package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.entity.InsectLoveScore;
import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import com.ssafy.bugar.domain.insect.enums.Category;
import com.ssafy.bugar.domain.insect.repository.InsectLoveScoreRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
public class RaisingInsectService {

    private final RaisingInsectRepository raisingInsectRepository;
    private final InsectLoveScoreRepository insectLoveScoreRepository;

    @Transactional
    public void save(Long userId, Long insectId, String nickname) {
        RaisingInsect raisingInsect = RaisingInsect.builder()
                .userId(userId)
                .insectId(insectId)
                .insectNickname(nickname)
                .build();

        raisingInsectRepository.save(raisingInsect);
    }

    @Transactional
    public void saveLoveScore(Long insectId, int categoryType) {
        Category category = null;
        if(categoryType == 1) {
            category = Category.FOOD;
        } else if (categoryType == 2) {
            category = Category.INTERACTION;
        } else if (categoryType == 3) {
            category = Category.WEATHER;
        }

        InsectLoveScore insectLoveScore = InsectLoveScore.builder()
                .insectId(insectId)
                .category(category)
                .build();

        insectLoveScoreRepository.save(insectLoveScore);
    }

}
