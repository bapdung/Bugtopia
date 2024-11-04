package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetInsectInfoResponseDto;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.entity.InsectLoveScore;
import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import com.ssafy.bugar.domain.insect.enums.AreaType;
import com.ssafy.bugar.domain.insect.enums.Category;
import com.ssafy.bugar.domain.insect.repository.AreaRepository;
import com.ssafy.bugar.domain.insect.repository.InsectLoveScoreRepository;
import com.ssafy.bugar.domain.insect.repository.InsectRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import java.util.List;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
public class RaisingInsectService {

    private final RaisingInsectRepository raisingInsectRepository;
    private final InsectLoveScoreRepository insectLoveScoreRepository;
    private final InsectRepository insectRepository;
    private final AreaRepository areaRepository;

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

    public GetAreaInsectResponseDto searchAreaInsect(Long userId, String areaName) {
        List<GetAreaInsectResponseDto.InsectList> insectList = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId, areaName);
        int num = insectList.size();
        return new GetAreaInsectResponseDto(num, insectList);
    }

    public GetInsectInfoResponseDto search(Long raisingInsectId) {
        RaisingInsect raisingInsect = raisingInsectRepository.findById(raisingInsectId).orElse(null);

        if(raisingInsect == null) {
            return null;
        }

        Insect insectType = insectRepository.findByInsectId(raisingInsect.getInsectId());
        AreaType areaName = areaRepository.findByAreaId(insectType.getAreaId()).getAreaName();

        GetInsectInfoResponseDto getInsectInfoResponseDto = GetInsectInfoResponseDto.builder()
                .areaType(areaName)
                .insectName(insectType.getInsectKrName())
                .canFeed(raisingInsect.getFeedCnt())
                .nickname(raisingInsect.getInsectNickname())
                .insectId(raisingInsect.getInsectId())
                .livingDate(raisingInsect.getCreatedDate())
                .clearEvent(raisingInsect.getEventId())
                .build();

        return getInsectInfoResponseDto;
    }
}
