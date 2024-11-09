package com.ssafy.bugar.domain.insect.entity;

import com.ssafy.bugar.domain.insect.enums.RaiseState;
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
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.ColumnDefault;
import org.hibernate.annotations.DynamicInsert;

@Entity
@Getter
@DynamicInsert
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "raising_insects")
public class RaisingInsect {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long raisingInsectId;

    @Column(nullable = false)
    private Long userId;

    @Column(nullable = false, length = 15)
    private String insectNickname;

    @Column(nullable = false)
    private Long insectId;

    @ColumnDefault("0")
    private Integer feedCnt;

    @ColumnDefault("0")
    private Integer interactCnt;

    @Column(nullable = false)
    @Enumerated(EnumType.STRING)
    private RaiseState state;

    @Column(nullable = false)
    private Timestamp createdDate;

    @Column(nullable = false)
    private Timestamp updatedDate;

    @ColumnDefault("1")
    private Integer continuousDays;

    @Column(nullable = false)
    private Long eventId;

    @Builder
    public RaisingInsect(Long userId, String insectNickname, Long insectId) {
        this.userId = userId;
        this.insectNickname = insectNickname;
        this.insectId = insectId;
        this.state = RaiseState.RAISE;
        this.createdDate = new Timestamp(System.currentTimeMillis());
        this.updatedDate = new Timestamp(System.currentTimeMillis());
        this.eventId = 1L;
    }

    public void updateClearEvent(long eventId) {
        this.eventId = eventId;
    }

    public void changeStatus(RaiseState state) {
        this.state = state;
    }

    public void updateFeedCnt() {
        this.feedCnt += 1;
    }

    public void updateInteractCnt() {
        this.interactCnt += 1;
    }

}
