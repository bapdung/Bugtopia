package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.response.CheckInsectEventResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetArInsectInfoResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetInsectInfoResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetInsectInfoResponseDto.Info;
import com.ssafy.bugar.domain.insect.dto.response.GetInsectInfoResponseDto.LoveScore;
import com.ssafy.bugar.domain.insect.dto.response.GetInsectInfoResponseDto.NextEventInfo;
import com.ssafy.bugar.domain.insect.dto.response.SaveRaisingInsectResponseDto;
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
import com.ssafy.bugar.domain.notification.enums.NotificationType;
import com.ssafy.bugar.domain.notification.service.NotificationService;
import com.ssafy.bugar.global.util.CategoryUtils;
import java.io.IOException;
import java.sql.Timestamp;
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
    private final NotificationService notificationService;

    @Transactional
    public SaveRaisingInsectResponseDto save(Long userId, Long insectId, String nickname) {
        RaisingInsect raisingInsect = RaisingInsect.builder()
                .userId(userId)
                .insectId(insectId)
                .insectNickname(nickname)
                .build();

        RaisingInsect savedRaisingInsect = raisingInsectRepository.save(raisingInsect);
        String family = insectRepository.findByInsectId(insectId).getFamily();

        return new SaveRaisingInsectResponseDto(savedRaisingInsect.getRaisingInsectId(), nickname, family);
    }

    @Transactional
    public CheckInsectEventResponseDto saveLoveScore(Long raisingInsectId, int categoryType) throws IOException {
        try {
            RaisingInsect raisingInsect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);
            Category category = CategoryUtils.getCategory(categoryType);

            InsectLoveScore insectLoveScore = InsectLoveScore.builder()
                    .raisingInsectId(raisingInsectId)
                    .category(category)
                    .build();

            insectLoveScoreRepository.save(insectLoveScore);

            if (category == Category.FOOD) {
                raisingInsect.updateFeedCnt();
            } else if (category == Category.INTERACTION) {
                raisingInsect.updateInteractCnt();
            }
        } catch (IllegalArgumentException e) {
            log.error(e.getMessage());
            throw e;
        }

        return checkInsectEvent(raisingInsectId);
    }

    public GetAreaInsectResponseDto searchAreaInsect(Long userId, String areaName) {
        List<GetAreaInsectResponseDto.InsectList> insectList = raisingInsectRepository.findInsectsByUserIdAndAreaName(
                userId, areaName);
        int num = insectList.size();
        return new GetAreaInsectResponseDto(num, insectList);
    }

    public GetInsectInfoResponseDto search(Long raisingInsectId) throws IOException {
        RaisingInsect raisingInsect = raisingInsectRepository.findById(raisingInsectId).orElse(null);

        if (raisingInsect == null) {
            return null;
        }

        // 곤충 정보 구하기
        Insect insectType = insectRepository.findByInsectId(raisingInsect.getInsectId());
        AreaType areaName = areaRepository.findByAreaId(insectType.getAreaId()).getAreaName();

        Info info = Info.builder()
                .raisingInsectId(raisingInsectId)
                .nickname(raisingInsect.getInsectNickname())
                .insectName(insectType.getInsectKrName())
                .family(insectType.getFamily())
                .areaType(areaName)
                .livingDate(raisingInsect.getCreatedDate())
                .build();

        // 애정도 정보 구하기
        List<InsectLoveScore> foodLoveScore = insectLoveScoreRepository.findInsectLoveScoreByCategory(raisingInsectId,
                Category.FOOD);

        Timestamp lastEat = null;

        if (!foodLoveScore.isEmpty()) {
            lastEat = foodLoveScore.get(0).getCreatedDate();
        }

        CheckInsectEventResponseDto checkInsectEvent = checkInsectEvent(raisingInsectId);
        int LoveScoreTotal = checkInsectEvent.getLoveScore();

        LoveScore loveScore = LoveScore.builder()
                .total(LoveScoreTotal)
                .feedCnt(raisingInsect.getFeedCnt())
                .lastEat(lastEat)
                .interactCnt(raisingInsect.getInteractCnt())
                .build();

        // 이벤트 정보 구하기
        NextEventInfo nextEventInfo = null;
        int completedEventScore = eventRepository.findByEventId(raisingInsect.getEventId()).getEventScore();
        List<Event> notCompletedEvents = eventRepository.getNotCompletedEvents(completedEventScore);

        if (notCompletedEvents.isEmpty()) {
            nextEventInfo = NextEventInfo.builder()
                    .nextEvent("END")
                    .build();
        } else {
            Event notCompletedEvent = notCompletedEvents.get(0);
            nextEventInfo = NextEventInfo.builder()
                    .nextEvent(notCompletedEvent.getEventName().toString())
                    .remainScore(notCompletedEvent.getEventScore() - LoveScoreTotal)
                    .build();
        }

        return new GetInsectInfoResponseDto(info, loveScore, nextEventInfo);
    }

    public CheckInsectEventResponseDto checkInsectEvent(Long raisingInsectId) throws IOException {
        RaisingInsect raisingInsect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);
        int score = calculateLoveScore(raisingInsect);

        // 진행할 이벤트가 있는지 여부와 이벤트 종류 확인
        int completedEventScore = eventRepository.findByEventId(raisingInsect.getEventId()).getEventScore();
        List<Event> notCompletedEventList = eventRepository.getNotCompletedEvents(completedEventScore);

        if (notCompletedEventList.isEmpty() || notCompletedEventList.get(0).getEventScore() > score) {
            return new CheckInsectEventResponseDto(score, false, null);
        }

        notificationService.save(raisingInsectId, NotificationType.ATTACK);
        return new CheckInsectEventResponseDto(score, true, notCompletedEventList.get(0).getEventName());
    }

    /**
     * 현재 애정도 점수 계산
     *
     * @param raisingInsect
     * @return score
     */
    public int calculateLoveScore(RaisingInsect raisingInsect) {
        // 연속 출석일에 따라 점수 추가 (최대 10점)
        // 애정도 올리기 항목에 따라 점수 추가 (WEATHER 5점, FOOD 3점, INTERACTION 1점)
        int score = (raisingInsect.getContinuousDays() <= 10) ? raisingInsect.getContinuousDays() : 10;
        List<InsectLoveScore> list = insectLoveScoreRepository.findInsectLoveScoreByRaisingInsectId(
                raisingInsect.getRaisingInsectId());

        for (InsectLoveScore insectLoveScore : list) {
            try {
                score += CategoryUtils.getCategoryScore(insectLoveScore.getCategory());
            } catch (IllegalArgumentException e) {
                log.error(e.getMessage());
                throw e;
            }
        }

        return score;
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

    public GetArInsectInfoResponseDto getInsectArInfo(Long raisingInsectId) {
        RaisingInsect raisingInsect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);
        Insect insect = insectRepository.findByInsectId(raisingInsect.getInsectId());
        List<InsectLoveScore> foodLoveScore = insectLoveScoreRepository.findInsectLoveScoreByCategory(raisingInsectId,
                Category.FOOD);

        return GetArInsectInfoResponseDto.builder()
                .raisingInsectId(raisingInsectId)
                .nickname(raisingInsect.getInsectNickname())
                .family(insect.getFamily())
                .feedCnt(raisingInsect.getFeedCnt())
                .lastEat(foodLoveScore.get(0).getCreatedDate())
                .interactCnt(raisingInsect.getInteractCnt())
                .build();
    }
}
