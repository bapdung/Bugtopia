package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.InsectLoveScore;
import com.ssafy.bugar.domain.insect.enums.Category;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

@Repository
public interface InsectLoveScoreRepository extends JpaRepository<InsectLoveScore, Long> {

    List<InsectLoveScore> findInsectLoveScoreByRaisingInsectId(Long raisingInsectId);

    @Query("""
                SELECT ls
                FROM InsectLoveScore ls
                WHERE ls.raisingInsectId = :raisingInsectId AND ls.category = :category
                ORDER BY ls.createdDate DESC
            """)
    List<InsectLoveScore> findInsectLoveScoreByCategory(Long raisingInsectId, Category category);

}
