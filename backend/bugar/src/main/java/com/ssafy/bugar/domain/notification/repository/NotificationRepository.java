package com.ssafy.bugar.domain.notification.repository;

import com.ssafy.bugar.domain.insect.dto.response.CatchInsectDetailResponseDto.MessageItem;
import com.ssafy.bugar.domain.notification.entity.Notification;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface NotificationRepository extends JpaRepository<Notification, Long> {

    @Query(value = """
                SELECT notification_id, type, is_read
                FROM notifications
                WHERE raising_insect_id = :raisingInsectId
                ORDER BY is_read ASC, created_date DESC
            """, nativeQuery = true)
    List<MessageItem> findNotificationListByRaisingInsectId(@Param("raisingInsectId") Long raisingInsectId);

    Notification findByNotificationId(Long notificationId);
}
