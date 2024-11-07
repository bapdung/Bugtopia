package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.request.CatchDeleteRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectDetailResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchListResponseDto;
import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.enums.CatchInsectDetailViewType;
import com.ssafy.bugar.domain.insect.enums.CatchInsectViewType;
import com.ssafy.bugar.domain.insect.repository.CatchingInsectRepository;
import com.ssafy.bugar.domain.insect.repository.EggRepository;
import com.ssafy.bugar.domain.insect.repository.InsectRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import com.ssafy.bugar.global.exception.CustomException;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
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
    private final CatchingBuilderService builderService;

    // 채집 도감 저장
    @Transactional
    public void save(Long userId, CatchSaveRequestDto request) {
        Insect insect = insectRepository.findById(request.getInsectId())
                .orElseThrow(() -> new CustomException(HttpStatus.NOT_FOUND,
                        "곤충 아이디를 찾지 못했습니다. 요청한 ID: " + request.getInsectId()));

        CatchedInsect catchingInsect = builderService.catchedInsectSaveBuilder(userId, request, insect);
        catchingInsectRepository.save(catchingInsect);
    }

    // 채집 곤충 목록 조회
    public CatchListResponseDto getCatchList(Long userId, String viewType) {
        CatchInsectViewType type = CatchInsectViewType.fromString(viewType);

        return switch (type) {
            case CATCHED -> builderService.catchedInsectListBuilder(userId);
            case RAISING -> builderService.raisingInsectListBuilder(userId);
            case DONE -> builderService.doneInsectListBuilder(userId);
            default -> throw new CustomException(HttpStatus.INTERNAL_SERVER_ERROR, "에러가 발생했습니다.");
        };
    }

    // 곤충 디테일 조회
    public CatchInsectDetailResponseDto getDetail(Long insectId, String viewType, Long userId) {
        CatchInsectDetailViewType type = CatchInsectDetailViewType.fromString(viewType);

        return switch (type) {
            case CATCHED -> builderService.catchedInsectDetailBuilder(insectId, userId);
            case DONE -> builderService.doneInsectDetailBuilder(insectId, userId);
            default -> throw new CustomException(HttpStatus.BAD_REQUEST, "ViewType 을 다시 한 번 확인해주세요.");
        };
    }

    @Transactional
    public void deleteCatchInsect(CatchDeleteRequestDto request) {
        CatchedInsect insect = catchingInsectRepository.findByCatchedInsectId(request.getCatchedInsectId());
        insect.deleteInsect(request.getCatchedInsectId());
    }
}

