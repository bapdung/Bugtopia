package com.ssafy.bugar.domain.insect.entity;

import com.ssafy.bugar.domain.insect.enums.RaiseState;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
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
@Table(name = "raising_insects")
public class RaisingInsect {

    @Id
    private Long raisingInsectId;

    @Column(nullable = false)
    private Long userId;

    @Column(nullable = false, length = 15)
    private String insectNickname;

    @Column(nullable = false)
    private Long insectId;

    @Column(nullable = false)
    @ColumnDefault("0")
    private int feedCnt;

    @Column(nullable = false)
    @ColumnDefault("0")
    private int interactCnt;

    @Column(nullable = false)
    private RaiseState state;

    @Column(nullable = false)
    private Timestamp createdDate;

    @Column(nullable = false)
    private Timestamp updatedDate;

    @Column(nullable = false)
    @ColumnDefault("1")
    private int continuousDays;

    @Column(nullable = false)
    private long eventId;

}
