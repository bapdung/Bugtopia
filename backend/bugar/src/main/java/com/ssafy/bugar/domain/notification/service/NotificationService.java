package com.ssafy.bugar.domain.notification.service;

import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import com.ssafy.bugar.domain.insect.repository.RaisingInsectRepository;
import com.ssafy.bugar.domain.notification.dto.response.NotificationResponseDto;
import com.ssafy.bugar.domain.notification.entity.Notification;
import com.ssafy.bugar.domain.notification.enums.NotificationType;
import com.ssafy.bugar.domain.notification.repository.NotificationRepository;
import com.ssafy.bugar.domain.user.entity.User;
import com.ssafy.bugar.domain.user.repository.UserRepository;
import java.io.IOException;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Slf4j
@RequiredArgsConstructor
public class NotificationService {

    private final NotificationRepository notificationRepository;
    private final FirebaseService firebaseService;
    private final UserRepository userRepository;
    private final RaisingInsectRepository raisingInsectRepository;

    // 알림 저장 및 생성 -> 푸시알림
    @Transactional
    public NotificationResponseDto save(Long raisingInsectId, NotificationType type) throws IOException {
        // 엔티티 가져오기
        RaisingInsect raisingInsect = raisingInsectRepository.findByRaisingInsectId(raisingInsectId);
        User user = userRepository.findByUserId(raisingInsect.getUserId());

        // Notification 저장
        Notification notification = new Notification(type, raisingInsect.getRaisingInsectId());
        notificationRepository.save(notification);

        // 푸시 알림 전송
        return push(user, raisingInsect, type);
    }

    // 알림 전송
    @Transactional
    public NotificationResponseDto push(User user, RaisingInsect raisingInsect, NotificationType type)
            throws IOException {
        // 메시지 생성
        String message = raisingInsect.getInsectNickname() + makeMessage(type);

        // Firebase 메시지 전송
        firebaseService.sendMessageTo(user.getDeviceId(), "벅토피아", message);

        return buildResponse(user, message);
    }

    @Transactional
    public NotificationResponseDto buildResponse(User user, String message) {
        return NotificationResponseDto.builder()
                .userId(user.getUserId())
                .userNickname(user.getNickname())
                .message(message)
                .build();
    }

    private String makeMessage(NotificationType type) {
        return switch (type) {
            case ATTACK -> "(이)의 영역에 누군가가 침범했어요!";
            case ATTENDANCE -> "(이)가 당신을 기다리고 있어요!";
            case BABY -> "(이)가 깜짝 선물을 보내왔어요!";
            case GREETING -> ": 스승님, 잘 지내시나요?";
            case HUNGRY -> "(이)가 배고파하고 있어요 ㅠ_ㅠ";
            case MEMORY -> "(이)가 당신을 떠올리고 있어요";
            case THANKS -> ": 스승님 덕분에 저는 잘 지내고 있어요!";
            default -> ""; // 기본값 반환 (필요에 따라 변경 가능)
        };
    }

    // 알림 읽기 처리
    public void readMessage(Long notificationId) {
        Notification notification = notificationRepository.findByNotificationId(notificationId);
        notification.read();
        notificationRepository.save(notification);
    }
}
