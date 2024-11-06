package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.request.CatchDeleteRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchDoneListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchDoneListResponseDto.DoneInsectItem;
import com.ssafy.bugar.domain.insect.dto.response.CatchListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchPossibleListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchPossibleListResponseDto.EggItem;
import com.ssafy.bugar.domain.insect.dto.response.CatchPossibleListResponseDto.PossibleInsect;
import com.ssafy.bugar.domain.insect.dto.response.CatchRaisingListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto.InsectList;
import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.enums.AreaType;
import com.ssafy.bugar.domain.insect.enums.CatchInsectViewType;
import com.ssafy.bugar.domain.insect.enums.CatchState;
import com.ssafy.bugar.domain.insect.repository.CatchingInsectRepository;
import com.ssafy.bugar.domain.insect.repository.EggRepository;
import com.ssafy.bugar.domain.insect.repository.InsectRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import java.util.List;
import java.util.Objects;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Slf4j
@RequiredArgsConstructor
public class CatchingInsectService {

    private final InsectRepository insectRepository;
    private final CatchingInsectRepository catchingInsectRepository;
    private final EggRepository eggRepository;
    private final RaisingInsectRepository raisingInsectRepository;

    @Transactional
    public void save(Long userId, CatchSaveRequestDto request) {
        Insect insect = insectRepository.findById(request.getInsectId())
                .orElseThrow(() -> new IllegalArgumentException("곤충 아이디를 찾지 못했습니다. " + request.getInsectId()));

        CatchedInsect catchingInsect = CatchedInsect.builder()
                .userId(userId)
                .insectId(request.getInsectId())
                .photo(request.getImgUrl())
                .state(Objects.requireNonNull(insect).isCanRaise() ? CatchState.POSSIBLE : CatchState.IMPOSSIBLE)
                .build();

        catchingInsectRepository.save(catchingInsect);
    }

    public CatchListResponseDto getCatchList(Long userId, CatchInsectViewType viewType) {


        // 육성 가능 곤충
        if (viewType == CatchInsectViewType.POSSIBLE) {
            CatchPossibleListResponseDto possibleResponse = getPossibleInsectList(userId);
            return CatchListResponseDto.builder().possibleInsectCnt(possibleResponse.getPossibleInsectCnt()).eggCnt(
                    possibleResponse.getEggCnt()).possibleList(possibleResponse.getPossibleList()).build();
        }

        // 육성 중 곤충
        if (viewType == CatchInsectViewType.RAISING) {
            CatchRaisingListResponseDto raisingResponse = getRaisingInsectList(userId);
            return CatchListResponseDto.builder().forestCnt(raisingResponse.getForestCnt()).waterCnt(
                    raisingResponse.getWaterCnt()).gardenCnt(raisingResponse.getGardenCnt()).forestList(raisingResponse.getForestList()).waterList(raisingResponse.getWaterList()).gardenList(raisingResponse.getGardenList()).build();
        }

        // 육성 완료 곤충
        CatchDoneListResponseDto doneResponse = getDoneInsectList(userId);
        return CatchListResponseDto.builder().totalCnt(doneResponse.getTotalCnt()).doneList(doneResponse.getDoneList()).build();
    }

    // 키우기 가능 곤충 + 알 목록 메서드
    public CatchPossibleListResponseDto getPossibleInsectList(Long userId) {
        List<PossibleInsect> possibleInsects = catchingInsectRepository.findPossibleInsectsByUserId(userId);
        List<EggItem> eggs = eggRepository.findEggItemsByUserIdOrderByCreatedDateDesc(userId);

        return CatchPossibleListResponseDto.builder().possibleInsectCnt(
                possibleInsects.size()).eggCnt(eggs.size()).possibleList(possibleInsects).eggList(eggs).build();
    }

    // 육성중 곤충 메서드
    public CatchRaisingListResponseDto getRaisingInsectList(Long userId) {
        List<InsectList> forestInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId, AreaType.FOREST.toString());
        List<InsectList> waterInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId, AreaType.WATER.toString());
        List<InsectList> gardenInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId, AreaType.GARDEN.toString());
        return CatchRaisingListResponseDto.builder().forestCnt(forestInsects.size()).waterCnt(
                waterInsects.size()).gardenCnt(gardenInsects.size()).forestList(forestInsects).waterList(waterInsects).gardenList(gardenInsects).build();
    }

    // 육성완료 곤충 메서드
    public CatchDoneListResponseDto getDoneInsectList(Long userId) {
        List<DoneInsectItem> doneInsects = raisingInsectRepository.findDoneInsectsByUserId(userId);
        return CatchDoneListResponseDto.builder().totalCnt(doneInsects.size()).doneList(doneInsects).build();
    }

    @Transactional
    public void deleteCatchInsect(CatchDeleteRequestDto request) {
        CatchedInsect insect = catchingInsectRepository.findByCatchedInsectId(request.getCatchedInsectId());
        insect.deleteInsect(request.getCatchedInsectId());
    }
}
