package com.ssafy.bugar.domain.insect.entity;

import com.ssafy.bugar.domain.insect.enums.EggState;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import java.sql.Timestamp;
import lombok.AccessLevel;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.DynamicInsert;

@Entity
@Getter
@DynamicInsert
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "eggs")
public class Egg {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long eggId;

    @Column(nullable = false, length = 15)
    private String parentNickname;

    @Column(nullable = false)
    private Timestamp createdDate;

    @Column(nullable = false)
    private Long insectId;

    @Column(nullable = false)
    private Long userId;

    @Column(nullable = false)
    private EggState state;

    public Egg(String parentNickname, Long insectId, Long userId) {
        this.parentNickname = parentNickname;
        this.insectId = insectId;
        this.userId = userId;
        this.createdDate = new Timestamp(System.currentTimeMillis());
        this.state = EggState.POSSIBLE;
    }

    public void raise() {
        this.state = EggState.RAISED;
    }
}
