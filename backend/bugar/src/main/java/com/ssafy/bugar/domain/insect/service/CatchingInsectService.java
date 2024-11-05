package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchListResponseDto.InsectItem;
import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.enums.CatchState;
import com.ssafy.bugar.domain.insect.repository.CatchingInsectRepository;
import com.ssafy.bugar.domain.insect.repository.InsectRepository;
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

    public CatchListResponseDto getCatchList(Long userId) {
        List<CatchedInsect> catchedInsects = catchingInsectRepository.findByUserId(userId);

        List<InsectItem> response = new ArrayList<>();
        for (CatchedInsect catchedInsect : catchedInsects) {
            Insect insect = insectRepository.findByInsectId(catchedInsect.getInsectId());
            InsectItem responseInsect = InsectItem.builder().insectName(insect.getInsectKrName()).photo(catchedInsect.getPhoto()).catchedDate(catchedInsect.getPhoto()).state(catchedInsect.getState()).build();
            response.add(responseInsect);
        }
        return CatchListResponseDto.builder().totalCnt(response.size()).insectList(response).build();
    }
}
