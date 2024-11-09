package com.ssafy.bugar.domain.notification.entity;

import com.ssafy.bugar.domain.notification.enums.NotificationType;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.sql.Timestamp;
import lombok.AccessLevel;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.ColumnDefault;
import org.hibernate.annotations.DynamicInsert;

@Entity
@Getter
@DynamicInsert
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "notifications")
public class Notification {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long notificationId;

    @Column(nullable = false)
    private Long raisingInsectId;

    @Column(nullable = false)
    @Enumerated(EnumType.STRING)
    private NotificationType type;

    @Column(nullable = false)
    @ColumnDefault("false")
    private boolean isRead;

    @Column(nullable = false)
    private Timestamp createdDate;

    public Notification(NotificationType type, Long raisingInsectId) {
        this.raisingInsectId = raisingInsectId;
        this.type = type;
        this.isRead = false;
        this.createdDate = new Timestamp(System.currentTimeMillis());
    }

    public void read() {
        this.isRead = true;
    }
}
