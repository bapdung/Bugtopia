package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.dto.response.CatchInsectListResponseDto.EggItem;
import com.ssafy.bugar.domain.insect.entity.Egg;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface EggRepository extends JpaRepository<Egg, Long> {

    @Query(value = """
            SELECT e.egg_id AS eggId, CONCAT(e.parent_nickname, ' 의 알') AS eggName, e.created_date AS receiveDate
            FROM eggs AS e
            WHERE e.user_id = :userId AND e.state = 0
            ORDER BY e.created_date DESC
            """, nativeQuery = true)
    List<EggItem> findEggItemsByUserIdOrderByCreatedDateDesc(@Param("userId") Long userId);

    Egg findByEggId(Long eggId);

}
