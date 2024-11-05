package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import com.ssafy.bugar.domain.insect.entity.Insect;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface CatchingInsectRepository extends JpaRepository<CatchedInsect, Long> {
    CatchedInsect findByCatchedInsectId(Long catchedInsectId);
    List<CatchedInsect> findByUserId(Long userId);
}
