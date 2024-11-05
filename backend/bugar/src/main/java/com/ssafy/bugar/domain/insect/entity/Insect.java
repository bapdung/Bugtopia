package com.ssafy.bugar.domain.insect.entity;

import com.ssafy.bugar.domain.insect.enums.Food;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.DynamicInsert;

@Entity
@Getter
@DynamicInsert
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "insects")
public class Insect {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long insectId;

    @Column(nullable = false, length = 50)
    private String insectKrName;

    @Column(nullable = false, length = 100)
    private String insectEngName;

    @Column(nullable = false, length = 512)
    private String insectInfo;

    @Column(nullable = false)
    private boolean canRaise;

    @Column(length = 200)
    private String rejectedReason;

    private Long areaId;

    @Column(nullable = false)
    @Enumerated(EnumType.STRING)
    private Food food;

    @Column(length = 100)
    private String family;

}
