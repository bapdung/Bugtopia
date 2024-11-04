package com.ssafy.bugar.domain.insect.entity;

import com.ssafy.bugar.domain.insect.enums.Category;
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
@Table(name = "insect_love_score")
public class InsectLoveScore {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long insectLoveScoreId;

    @Column(nullable = false)
    private Long collectedInsectId;

    @Column(nullable = false)
    @Enumerated(EnumType.STRING)
    private Category category;

    @Column(nullable = false)
    private Timestamp cratedDate;

    @Builder
    public InsectLoveScore(Long insectId, Category category) {
        this.collectedInsectId = insectId;
        this.category = category;
        this.cratedDate = new Timestamp(System.currentTimeMillis());
    }

}
