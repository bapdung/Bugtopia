package com.ssafy.bugar.domain.insect.service;

import com.ssafy.bugar.domain.insect.dto.request.CatchDeleteRequestDto;
import com.ssafy.bugar.domain.insect.dto.request.CatchSaveRequestDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectDetailResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.SearchInsectResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.SearchInsectResponseDto.Content;
import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.enums.CatchInsectDetailViewType;
import com.ssafy.bugar.domain.insect.enums.CatchInsectViewType;
import com.ssafy.bugar.domain.insect.enums.CatchState;
import com.ssafy.bugar.domain.insect.repository.AreaRepository;
import com.ssafy.bugar.domain.insect.repository.CatchingInsectRepository;
import com.ssafy.bugar.domain.insect.repository.EggRepository;
import com.ssafy.bugar.domain.insect.repository.InsectRepository;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import com.ssafy.bugar.global.exception.CustomException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.ParameterizedTypeReference;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.client.RestTemplate;

@Service
@Slf4j
@RequiredArgsConstructor
public class CatchingInsectService {

    private final RestTemplate restTemplate;
    private final AreaRepository areaRepository;

    @Value("${fastapi.base_url}/api/insects-detection")
    private String fastApiUrl;

    private final InsectRepository insectRepository;
    private final CatchingInsectRepository catchingInsectRepository;
    private final EggRepository eggRepository;
    private final RaisingInsectRepository raisingInsectRepository;
    private final CatchingBuilderService builderService;

    @Transactional
    public void save(Long userId, CatchSaveRequestDto request) {
        Insect insect = insectRepository.findById(request.getInsectId())
                .orElseThrow(() -> new CustomException(HttpStatus.NOT_FOUND,
                        "곤충 아이디를 찾지 못했습니다. 요청한 ID: " + request.getInsectId()));

        CatchedInsect catchingInsect = CatchedInsect.builder()
                .userId(userId)
                .insectId(request.getInsectId())
                .photo(request.getImgUrl())
                .state(Objects.requireNonNull(insect).isCanRaise() ? CatchState.POSSIBLE : CatchState.IMPOSSIBLE)
                .build();

        catchingInsectRepository.save(catchingInsect);
    }

    // 채집 곤충 목록 조회
    public CatchInsectListResponseDto getCatchList(Long userId, String viewType) {
        CatchInsectViewType type = CatchInsectViewType.fromString(viewType);

        return switch (type) {
            case CATCHED -> builderService.catchedInsectListBuilder(userId);
            case RAISING -> builderService.raisingInsectListBuilder(userId);
            case DONE -> builderService.doneInsectListBuilder(userId);
        };
    }

    // 곤충 디테일 정보
    public CatchInsectDetailResponseDto getDetail(Long insectId, String viewType, Long userId) {
        CatchInsectDetailViewType type = CatchInsectDetailViewType.fromString(viewType);

        return switch (type) {
            case CATCHED -> builderService.catchedInsectDetailBuilder(insectId, userId);
            case DONE -> builderService.doneInsectDetailBuilder(insectId);
        };
    }

    @Transactional
    public void deleteCatchInsect(CatchDeleteRequestDto request) {
        CatchedInsect insect = catchingInsectRepository.findByCatchedInsectId(request.getCatchedInsectId());
        insect.deleteInsect(request.getCatchedInsectId());
    }

    public SearchInsectResponseDto search(long userId, String imgUrl) {
        // FastApi 요청 생성
        Map<String, String> request = new HashMap<>();
        request.put("img_url", imgUrl);

        HttpHeaders headers = new HttpHeaders();
        HttpEntity<Map<String, String>> entity = new HttpEntity<>(request, headers);

        ResponseEntity<Map<String, Object>> response = restTemplate.exchange(
                fastApiUrl, HttpMethod.POST, entity, new ParameterizedTypeReference<Map<String, Object>>() {
                }
        );

        // FastAPI 응답 처리
        int status = (int) response.getBody().get("status");
        if (status == 200) {
            String name = (String) response.getBody().get("content");

            // 곤충 정보 찾기
            Insect insect = insectRepository.findByInsectEngName(name);

            if (insect == null) {
                return new SearchInsectResponseDto(404, null);
            }

            // 지역 찾기
            long areaId = insect.getAreaId();
            String areaName = areaRepository.findByAreaId(areaId).getAreaName() + "";

            // 육성 가능 여부 확인
            int canRaise = -1;
            if (!insect.isCanRaise()) {
                canRaise = 1;
            } else {
                List<GetAreaInsectResponseDto.InsectList> list = raisingInsectRepository.findInsectsByUserIdAndAreaName(
                        userId, areaName);

                if (list.size() >= 3) {
                    canRaise = 2;
                } else {
                    canRaise = 0;
                }
            }

            // response
            Content content = Content.builder()
                    .insectId(insect.getInsectId())
                    .krName(insect.getInsectKrName())
                    .engName(insect.getInsectEngName())
                    .info(insect.getInsectInfo())
                    .imgUrl(imgUrl)
                    .canRaise(canRaise)
                    .family(insect.getFamily())
                    .area(areaName)
                    .rejectedReason(insect.getRejectedReason())
                    .build();

            return new SearchInsectResponseDto(200, content);
        }

        return new SearchInsectResponseDto(404, null);
    }
}

