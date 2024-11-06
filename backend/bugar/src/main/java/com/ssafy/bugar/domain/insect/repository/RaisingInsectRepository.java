package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.dto.response.CatchDoneListResponseDto;
import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto;
import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import java.util.List;
import org.springframework.stereotype.Repository;

@Repository
public interface RaisingInsectRepository extends JpaRepository<RaisingInsect, Long> {

    @Query(value = """
            SELECT ri.raising_insect_id AS raisingInsectId, ri.insect_id AS insectId, ri.insect_nickname AS nickname, i.family AS family
            FROM raising_insects ri
            JOIN insects i ON ri.insect_id = i.insect_id
            JOIN area a ON i.area_id = a.area_id
            WHERE ri.user_id = :userId AND a.area_name = :areaName
            """, nativeQuery = true)
    List<GetAreaInsectResponseDto.InsectList> findInsectsByUserIdAndAreaName(@Param("userId") Long userId, @Param("areaName") String areaName);

    @Query(value = """
            SELECT r.raising_insect_id AS raisingInsectId, r.insect_nickname AS insectNickname, i.family AS family
            FROM raising_insects AS r
            JOIN insects AS i ON i.insect_id = r.insect_id
            WHERE r.state = 'DONE' AND r.user_id = :userId
            ORDER BY r.updated_date DESC
            """, nativeQuery = true)
    List<CatchDoneListResponseDto.DoneInsectItem> findDoneInsectsByUserId(@Param("userId") Long userId);

    RaisingInsect findByRaisingInsectId(Long raisingInsectId);
}
