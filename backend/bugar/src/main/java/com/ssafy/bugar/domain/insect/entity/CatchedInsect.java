package com.ssafy.bugar.domain.insect.entity;

import com.ssafy.bugar.domain.insect.enums.CatchState;
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
import org.hibernate.annotations.DynamicInsert;

@Entity
@Getter
@DynamicInsert
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "catched_insects")
public class CatchedInsect {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long catchedInsectId;

    @Column(nullable = false)
    private Long userId;

    @Column(nullable = false)
    private Long insectId;

    @Column(nullable = false)
    private Timestamp createdDate;

    @Column(nullable = false, length = 512)
    private String photo;

    @Column(nullable = false)
    @Enumerated(EnumType.STRING)
    private CatchState state;

    @Builder
    public CatchedInsect(Long userId, Long insectId, String photo, CatchState state) {
        this.userId = userId;
        this.insectId = insectId;
        this.photo = photo;
        this.state = state;
        this.createdDate = new Timestamp(System.currentTimeMillis());
    }

    public void deleteInsect(Long catchedInsectId) {
        this.state = CatchState.DELETE;
    }
}
