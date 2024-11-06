package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.dto.response.CatchPossibleListResponseDto.PossibleInsect;
import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import com.ssafy.bugar.domain.insect.entity.Insect;
import com.ssafy.bugar.domain.insect.enums.CatchState;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface CatchingInsectRepository extends JpaRepository<CatchedInsect, Long> {
    CatchedInsect findByCatchedInsectId(Long catchedInsectId);

    @Query(value = """
            SELECT c.catched_insect_id AS catchedInsectId, c.photo AS photo, c.created_date AS catchedDate, i.insect_kr_name AS insectName
            FROM catched_insects AS c
            JOIN insects AS i ON i.insect_id = c.insect_id
            WHERE c.user_id = :userId
            ORDER BY c.created_date DESC
            """, nativeQuery = true)
    List<PossibleInsect> findPossibleInsectsByUserId(@Param("userId") Long userId);
}
