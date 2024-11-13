package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectDetailResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectDetailResponseDto.CatchInsectDetailProjection;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectDetailResponseDto.MessageItem;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectListResponseDto.CatchedInsectItem;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectListResponseDto.DoneInsectItem;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectListResponseDto.EggItem;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto.InsectList;
import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.enums.AreaType;
import com.ssafy.bugar.domain.insect.enums.CatchState;
import com.ssafy.bugar.domain.insect.repository.CatchingInsectRepository;
import com.ssafy.bugar.domain.insect.repository.EggRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import com.ssafy.bugar.domain.notification.repository.NotificationRepository;
import java.util.List;
import java.util.Objects;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

@Service
@Slf4j
@RequiredArgsConstructor
public class CatchingBuilderService {

    private final NotificationRepository notificationRepository;
    private final CatchingInsectRepository catchingInsectRepository;
    private final EggRepository eggRepository;
    private final RaisingInsectRepository raisingInsectRepository;

    // 채집 곤충 도감 저장
    public CatchedInsect catchedInsectSaveBuilder(Long userId, CatchSaveRequestDto request, Insect insect) {
        return CatchedInsect.builder()
                .userId(userId)
                .insectId(request.getInsectId())
                .photo(request.getImgUrl())
                .state(Objects.requireNonNull(insect).isCanRaise() ? CatchState.POSSIBLE : CatchState.IMPOSSIBLE)
                .build();
    }

    // 육성 가능 곤충 목록
    public CatchInsectListResponseDto catchedInsectListBuilder(Long userId) {
        List<CatchedInsectItem> possibleInsects = catchingInsectRepository.findPossibleInsectsByUserId(userId);
        List<EggItem> eggs = eggRepository.findEggItemsByUserIdOrderByCreatedDateDesc(userId);

        return CatchInsectListResponseDto.builder()
                .catchedInsectCnt(possibleInsects.size())
                .catchList(possibleInsects)
                .eggCnt(eggs.size())
                .eggList(eggs)
                .build();
    }

    // 육성중 곤충 목록
    public CatchInsectListResponseDto raisingInsectListBuilder(Long userId) {
        List<InsectList> forestInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId,
                AreaType.FOREST.toString());
        List<InsectList> waterInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId,
                AreaType.WATER.toString());
        List<InsectList> gardenInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId,
                AreaType.GARDEN.toString());

        return CatchInsectListResponseDto.builder()
                .forestCnt(forestInsects.size())
                .forestList(forestInsects)
                .waterCnt(waterInsects.size())
                .waterList(waterInsects)
                .gardenCnt(gardenInsects.size())
                .gardenList(gardenInsects)
                .build();
    }

    // 육성 완료 곤충 목록
    public CatchInsectListResponseDto doneInsectListBuilder(Long userId) {
        List<DoneInsectItem> doneInsects = raisingInsectRepository.findDoneInsectsByUserId(userId);
        return CatchInsectListResponseDto.builder()
                .totalCnt(doneInsects.size())
                .doneList(doneInsects)
                .build();
    }

    // 보유 곤충 개수 확인
    public int getHavingInsectCnt(CatchInsectDetailProjection catchInsect, Long userId) {
        return raisingInsectRepository.findInsectsByUserIdAndAreaName(userId, String.valueOf(catchInsect.getArea()))
                .size();
    }

    // 키우기 가능 여부 반환
    public int getCanRaise(CatchInsectDetailProjection catchInsect, Long userId) {

        int havingInsectCnt = getHavingInsectCnt(catchInsect, userId);

        // 키우기 가능 여부 확인 로직
        return catchInsect.getCanRaise() == 0 ? (havingInsectCnt >= 3 ? 2 : 0) : catchInsect.getCanRaise();

    }

    // 채집 곤충 상세 정보
    CatchInsectDetailResponseDto catchedInsectDetailBuilder(Long catchedInsectId, Long userId) {
        CatchInsectDetailProjection catchInsect = catchingInsectRepository.findCatchedInsectDetail(catchedInsectId);

        return CatchInsectDetailResponseDto.builder()
                .krName(catchInsect.getKrName())
                .engName(catchInsect.getEngName())
                .info(catchInsect.getInfo())
                .imgUrl(catchInsect.getPhoto())
                .canRaise(getCanRaise(catchInsect, userId))
                .family(catchInsect.getFamily())
                .area(catchInsect.getArea())
                .rejectedReason(catchInsect.getRejectedReason())
                .build();
    }

    // 육성 완료 곤충 상세 정보
    CatchInsectDetailResponseDto doneInsectDetailBuilder(Long raisingInsectId) {
        CatchInsectDetailProjection doneInsect = raisingInsectRepository.findDoneInsectDetail(raisingInsectId);
        List<MessageItem> messages = notificationRepository.findNotificationListByRaisingInsectId(raisingInsectId);
        log.info(messages.get(0).getNotificationId().toString() + " 알림 ID@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        log.info(messages.get(0).getType() + " 알림 타입@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        log.info(messages.get(0).getIsRead() + " 읽음여부@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        return CatchInsectDetailResponseDto.builder()
                .insectNickname(doneInsect.getInsectNickname())
                .krName(doneInsect.getKrName())
                .startDate(doneInsect.getStartDate())
                .doneDate(doneInsect.getDoneDate())
                .meetingDays(doneInsect.getMeetingDays())
                .family(doneInsect.getFamily())
                .messageCnt(messages.size())
                .messages(messages)
                .build();
    }
}
