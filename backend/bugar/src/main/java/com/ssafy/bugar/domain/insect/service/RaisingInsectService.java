package com.ssafy.bugar.domain.insect.service;

import static java.rmi.server.LogStream.log;

import com.ssafy.bugar.domain.insect.dto.response.CheckInsectEventResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetInsectInfoResponseDto;
import com.ssafy.bugar.domain.insect.entity.Event;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.entity.InsectLoveScore;
import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import com.ssafy.bugar.domain.insect.enums.AreaType;
import com.ssafy.bugar.domain.insect.enums.Category;
import com.ssafy.bugar.domain.insect.enums.EventType;
import com.ssafy.bugar.domain.insect.enums.RaiseState;
import com.ssafy.bugar.domain.insect.repository.AreaRepository;
import com.ssafy.bugar.domain.insect.repository.EventRepository;
import com.ssafy.bugar.domain.insect.repository.InsectLoveScoreRepository;
import com.ssafy.bugar.domain.insect.repository.InsectRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import com.ssafy.bugar.global.util.CategoryUtils;
import java.util.List;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@RequiredArgsConstructor
@Slf4j
public class RaisingInsectService {

    private final RaisingInsectRepository raisingInsectRepository;
    private final InsectLoveScoreRepository insectLoveScoreRepository;
    private final InsectRepository insectRepository;
    private final AreaRepository areaRepository;
    private final EventRepository eventRepository;

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
        try {
            Category category = CategoryUtils.getCategory(categoryType);

            InsectLoveScore insectLoveScore = InsectLoveScore.builder()
                    .insectId(insectId)
                    .category(category)
                    .build();

            insectLoveScoreRepository.save(insectLoveScore);
        } catch (IllegalArgumentException e) {
            log.error(e.getMessage());
            throw e;
        }
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

        List<InsectLoveScore> foodLoveScore = insectLoveScoreRepository.findInsectLoveScoreByCategory(Category.FOOD);

        CheckInsectEventResponseDto checkInsectEvent = checkInsectEvent(raisingInsectId);

        return GetInsectInfoResponseDto.builder()
                .raisingInsectId(raisingInsectId)
                .nickname(raisingInsect.getInsectNickname())
                .insectName(insectType.getInsectKrName())
                .family(insectType.getFamily())
                .areaType(areaName)
                .feedCnt(raisingInsect.getFeedCnt())
                .lastEat(foodLoveScore.get(0).getCreatedDate())
                .interactCnt(raisingInsect.getInteractCnt())
                .livingDate(raisingInsect.getCreatedDate())
                .continuousDays(raisingInsect.getContinuousDays())
                .loveScore(checkInsectEvent.getLoveScore())
                .isEvent(checkInsectEvent.isEvnet())
                .eventType(checkInsectEvent.getEventType())
                .build();
    }

    public CheckInsectEventResponseDto checkInsectEvent(Long raisingInsectId) {
        RaisingInsect insect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);

        // 현재 애정도 점수 계산
        // 연속 출석일에 따라 점수 추가 (최대 10점)
        // 애정도 올리기 항목에 따라 점수 추가 (WEATHER 5점, FOOD 3점, INTERACTION 1점)
        int score = (insect.getContinuousDays() <= 10) ? insect.getContinuousDays() : 10;
        List<InsectLoveScore> list = insectLoveScoreRepository.findInsectLoveScoreByRaisingInsectId(raisingInsectId);

        for(InsectLoveScore insectLoveScore : list) {
            try {
                score += CategoryUtils.getCategoryScore(insectLoveScore.getCategory());
            } catch (IllegalArgumentException e) {
                log.error(e.getMessage());
                throw e;
            }
        }

        // 진행할 이벤트가 있는지 여부와 이벤트 종류 확인
        int completedEventScore = eventRepository.findByEventId(insect.getEventId()).getEventScore();
        List<Event> notCompletedEventList = eventRepository.getNotCompletedEvents(completedEventScore);

        if(notCompletedEventList.isEmpty()) {
            return new CheckInsectEventResponseDto(score, false, null);
        }

        return new CheckInsectEventResponseDto(score, true, notCompletedEventList.get(0).getEventName());
    }

    @Transactional
    public void clearEvent(long raisingInsectId, EventType clearEventType) {
        RaisingInsect raisingInsect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);
        Event clearEvent = eventRepository.findByEventName(clearEventType);
        raisingInsect.updateClearEvent(clearEvent.getEventId());
    }

    @Transactional
    public void release(long raisingInsectId) {
        RaisingInsect raisingInsect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);
        raisingInsect.changeStatus(RaiseState.RELEASE);
    }
}
