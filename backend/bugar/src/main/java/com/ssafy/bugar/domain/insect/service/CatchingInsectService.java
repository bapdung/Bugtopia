package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.request.CatchDeleteRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchPossibleListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchPossibleListResponseDto.EggItem;
import com.ssafy.bugar.domain.insect.dto.response.CatchPossibleListResponseDto.PossibleInsect;
import com.ssafy.bugar.domain.insect.dto.response.CatchRaisingListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto.InsectList;
import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import com.ssafy.bugar.domain.insect.entity.Egg;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.enums.AreaType;
import com.ssafy.bugar.domain.insect.enums.CatchInsectViewType;
import com.ssafy.bugar.domain.insect.enums.CatchState;
import com.ssafy.bugar.domain.insect.repository.CatchingInsectRepository;
import com.ssafy.bugar.domain.insect.repository.EggRepository;
import com.ssafy.bugar.domain.insect.repository.InsectRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import java.util.ArrayList;
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
//        List<CatchedInsect> catchedInsects = catchingInsectRepository.findByUserId(userId);
//        List<InsectItem> response = new ArrayList<>();
//        for (CatchedInsect catchedInsect : catchedInsects) {
//            Insect insect = insectRepository.findByInsectId(catchedInsect.getInsectId());
//            InsectItem responseInsect = InsectItem.builder().insectName(insect.getInsectKrName()).photo(catchedInsect.getPhoto()).catchedDate(catchedInsect.getPhoto()).state(catchedInsect.getState()).build();
//            response.add(responseInsect);
//        }
//        return CatchListResponseDto.builder().totalCnt(response.size()).insectList(response).build();
        return CatchListResponseDto.builder().build();
    }

    // 키우기 가능 곤충 + 알 목록 return
    public CatchPossibleListResponseDto getPossibleInsectList(Long userId) {
        List<CatchedInsect> catchedInsects = catchingInsectRepository.findByUserIdAndStateOrderByCatchedDateDesc(userId, CatchState.POSSIBLE);
        List<PossibleInsect> possibleInsects = new ArrayList<>();
        for (CatchedInsect catchedInsect : catchedInsects) {
            Insect insect = insectRepository.findByInsectId(catchedInsect.getInsectId());
            PossibleInsect possibleInsect = PossibleInsect.builder().catchedInsectId(catchedInsect.getCatchedInsectId()).insectName(insect.getInsectKrName()).photo(
                    catchedInsect.getPhoto()).catchedDate(String.valueOf(catchedInsect.getCatchedDate())).build();
            possibleInsects.add(possibleInsect);
        }
        List<EggItem> eggs = new ArrayList<>();
        List<Egg> findEggs = eggRepository.findByUserIdOrderByCreatedDateDesc(userId);
        for (Egg findEgg : findEggs) {
            EggItem egg = EggItem.builder().eggId(findEgg.getEggId()).eggName(findEgg.getParentNickname() + " 의 알").receiveDate(findEgg.getCreatedDate().toString()).build();
            eggs.add(egg);
        }

        return CatchPossibleListResponseDto.builder().possibleInsectCnt(
                possibleInsects.size()).eggCnt(eggs.size()).possibleList(possibleInsects).eggList(eggs).build();
    }

    // 육성중 곤충
    public CatchRaisingListResponseDto getRaisingInsectList(Long userId) {
        List<InsectList> forestInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId, AreaType.FOREST.toString());
        List<InsectList> waterInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId, AreaType.WATER.toString());
        List<InsectList> gardenInsects = raisingInsectRepository.findInsectsByUserIdAndAreaName(userId, AreaType.GARDEN.toString());
        return CatchRaisingListResponseDto.builder().forestCnt(forestInsects.size()).waterCnt(
                waterInsects.size()).gardenCnt(gardenInsects.size()).forestList(forestInsects).waterList(waterInsects).gardenList(gardenInsects).build();
    }

    // 육성완료 곤충
//    public CatchDoneListResponseDto getDoneInsectList(Long userId) {
//        List<>
//    }

    @Transactional
    public void deleteCatchInsect(CatchDeleteRequestDto request) {
        CatchedInsect insect = catchingInsectRepository.findByCatchedInsectId(request.getCatchedInsectId());
        insect.deleteInsect(request.getCatchedInsectId());
    }
}
