package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.InsectLoveScore;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface InsectLoveScoreRepository extends JpaRepository<InsectLoveScore, Long> {

    List<InsectLoveScore> findInsectLoveScoreByCollectedInsectId(Long raisingInsectId);

}
