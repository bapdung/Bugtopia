package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.dto.response.CatchInsectDetailResponseDto.CatchInsectDetailProjection;
import com.ssafy.bugar.domain.insect.dto.response.CatchInsectListResponseDto.CatchedInsectItem;
import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
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
            WHERE c.user_id = :userId AND c.state IN ('POSSIBLE', 'IMPOSSIBLE')
            ORDER BY c.created_date DESC
            """, nativeQuery = true)
    List<CatchedInsectItem> findPossibleInsectsByUserId(@Param("userId") Long userId);

    @Query(value = """
                SELECT i.insect_kr_name AS krName, i.insect_eng_name AS engName, i.insect_info AS info, i.family as family, a.area_name as area, 
                       IF(c.state = 'POSSIBLE', 0, 1) AS canRaise, i.rejected_reason AS rejectedReason, c.photo AS photo
                FROM catched_insects AS c
                JOIN insects AS i ON i.insect_id = c.insect_id
                JOIN area AS a ON a.area_id = i.area_id
                WHERE c.catched_insect_id = :catchedInsectId
            """, nativeQuery = true)
    CatchInsectDetailProjection findCatchedInsectDetail(@Param("catchedInsectId") Long catchedInsectId);
}
